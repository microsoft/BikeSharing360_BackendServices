
CREATE VIEW [dbo].[vwRides] AS
SELECT rid.Id
	, rid.Duration
	, rid.Start
	, rid.StartStationId
	, rid.EndStationId
	, rid.BikeId
	, rid.UserId
	, rid.EventType
	, rid.EventId
	, rid.GeoDistance
	, DATEPART(hh, rid.Start) AS HourOfDay
	, DATENAME(w, rid.Start) AS DayName
	, DATENAME(m, rid.Start) AS MonthName
	, CASE WHEN DATEPART(w, rid.Start) IN (1, 7) THEN 1 ELSE 0 END AS IsWeekend
	, CASE WHEN DATEPART(w, rid.Start) = 1 OR hol.Date IS NOT NULL THEN 1 ELSE 0 END AS IsHoliday
	, COALESCE(hol.Name, '') AS HolidayName
FROM dbo.rides rid
	LEFT JOIN dbo.holidayDates hol ON CAST(rid.Start AS Date) = hol.Date