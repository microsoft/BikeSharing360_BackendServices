
CREATE VIEW [dbo].[vwBikeLogistics] AS
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
		SELECT DISTINCT CAST(Start AS DATE) AS Date
			, hour.HourOfDay AS HourOfDay
		FROM dbo.rides rid
		CROSS JOIN (values(0),(1),(2),(3),(4),(5),(6),(7),(8),(9),(10),(11),(12), 
			(13),(14),(15),(16),(17),(18),(19),(20),(21),(22),(23)) as hour(HourOfDay)
		) dat
	LEFT JOIN dbo.holidayDates hol ON dat.Date = hol.Date
	)
, Arrivals AS
	(
	SELECT EndStationId
		, CAST(Start AS DATE) AS Date
		, HourOfDay
		, COUNT(*) AS Arrivals
	FROM dbo.vwRides
	GROUP BY EndStationId
		, CAST(Start AS DATE)
		, HourOfDay
	)
, Departures AS
	(
	SELECT StartStationId
		, CAST(Start AS DATE) AS Date
		, HourOfDay
		, COUNT(*) AS Departures
	FROM dbo.vwRides
	GROUP BY StartStationId
		, CAST(Start AS DATE)
		, HourOfDay
	)
, Origin AS	
	(
	SELECT sta.Id AS StationId
		, dat.Date
		, dat.DayName
		, dat.MonthName
		, dat.HourOfDay
		, dat.IsWeekend
		, dat.IsHoliday
		, dat.HolidayName
		, SUM(COALESCE(Arrivals, 0)) AS Arrivals
		, SUM(COALESCE(Departures, 0)) AS Departures
	FROM dbo.stations sta
		CROSS JOIN Datetimes dat	
		LEFT JOIN Arrivals arr ON sta.Id = arr.EndStationId AND dat.Date = arr.Date AND dat.HourOfDay = arr.HourOfDay
		LEFT JOIN Departures dep ON sta.Id = dep.StartStationId AND dat.Date = dep.Date AND dat.HourOfDay = dep.HourOfDay
	GROUP BY sta.Id
		, dat.Date
		, dat.DayName
		, dat.MonthName
		, dat.HourOfDay
		, dat.IsWeekend
		, dat.IsHoliday
		, dat.HolidayName
	)
, Events AS
	(
	SELECT ven.StationId
		, CAST(eve.StartTime AS DATE) AS EventDate
		, DATEPART(hh, DATEADD(hh, -1, eve.StartTime)) AS HourOfDay
		, COUNT(*) AS EventCount
	FROM dbo.vwVenueStations ven
		JOIN [dbo].[Events] eve ON ven.VenueId = eve.VenueId
	GROUP BY StationId
		, CAST(eve.StartTime AS DATE)
		, DATEPART(hh, DATEADD(hh, -1, eve.StartTime))
	)

SELECT ori.StationId
	, ori.Date
	, ori.DayName
	, ori.MonthName
	, ori.HourOfDay
	, ori.IsWeekend
	, ori.IsHoliday
	, ori.HolidayName
	, ori.Arrivals
	, ori.Departures
	, COALESCE(eve.EventCount, 0) AS EventCount
FROM
	Origin AS ori
LEFT JOIN Events eve ON ori.StationId = eve.StationId 
	AND ori.Date = eve.EventDate 
	AND ori.HourOfDay = eve.HourOfDay