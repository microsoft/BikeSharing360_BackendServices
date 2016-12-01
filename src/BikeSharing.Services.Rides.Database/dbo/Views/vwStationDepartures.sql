
CREATE VIEW dbo.vwStationDepartures AS
SELECT TOP 100 *
	, geography::STPointFromText('POINT(' + CAST(Longitude AS VARCHAR(20)) + ' ' + CAST(Latitude AS VARCHAR(20)) + ')', 4326) AS GeoPosition
FROM
	(
	SELECT rid.StartStationId
		, des.Name
		, des.Latitude
		, des.Longitude
		, COUNT(1) AS Departures
	FROM dbo.rides rid
		JOIN stations des ON rid.StartStationId = des.Id
	GROUP BY rid.StartStationId
	, des.Name
	, des.Latitude
	, des.Longitude
	) info
ORDER BY Departures desc