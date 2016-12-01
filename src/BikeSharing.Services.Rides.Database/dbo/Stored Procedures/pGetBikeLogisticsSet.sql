
CREATE PROCEDURE [dbo].[pGetBikeLogisticsSet]
	@PDate DATE = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @PDate = COALESCE(@PDate, DATEADD(dd, 1, GETDATE()));

	WITH DateTimes AS	
		(
		SELECT dat.Date
			, dat.HourOfDay
			, DATENAME(w, dat.Date) AS DayName
			, DATENAME(m, dat.Date) AS MonthName
			, CASE WHEN DATEPART(w, dat.Date) IN (1, 7) THEN 1 ELSE 0 END AS IsWeekend
			, CASE WHEN DATEPART(w, dat.Date) = 1 OR hol.Date IS NOT NULL THEN 1 ELSE 0 END AS IsHoliday
			, COALESCE(hol.Name, '') AS HolidayName
		FROM 
			(
			SELECT @PDate AS Date
				, hour.HourOfDay AS HourOfDay
			FROM (values(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12), 
				(13),(14),(15),(16),(17),(18),(19),(20),(21),(22),(23)) as hour(HourOfDay)
			) dat
		LEFT JOIN dbo.holidayDates hol ON dat.Date = hol.Date
		)
	, Events AS
		(
		SELECT sta.Id as StationId
		, dat.Date
		, dat.DayName
		, dat.MonthName
		, dat.HourOfDay
		, dat.IsWeekend
		, dat.IsHoliday
		, dat.HolidayName
		, 0 AS Arrivals
		, 0 AS Departures
		, CASE WHEN eve.Name IS NULL THEN 0 ELSE 1 END AS EventCount
		FROM  dbo.stations sta 
		CROSS APPLY Datetimes dat
		LEFT JOIN dbo.vwVenueStations ven ON sta.Id = ven.StationId
		LEFT JOIN dbo.Events eve ON ven.VenueId = eve.VenueId 
			AND CAST(eve.StartTime AS DATE) = dat.Date 
			AND dat.HourOfDay = DATEPART(hh, DATEADD(hh, -1, eve.StartTime))
		)
	SELECT StationId
		, Date
		, DayName
		, MonthName
		, HourOfDay
		, IsWeekend
		, IsHoliday
		, HolidayName
		, SUM(Arrivals) AS Arrivals
		, SUM(Departures) AS Departures
		, SUM(EventCount) AS EventCount
	FROM Events eve
	GROUP BY StationId
		, Date
		, DayName
		, MonthName
		, HourOfDay
		, IsWeekend
		, IsHoliday
		, HolidayName
END