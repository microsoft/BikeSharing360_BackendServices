
CREATE VIEW dbo.vwStationArrivals AS
SELECT TOP 100 *
	, geography::STPointFromText('POINT(' + CAST(Longitude AS VARCHAR(20)) + ' ' + CAST(Latitude AS VARCHAR(20)) + ')', 4326) AS GeoPosition
FROM
	(
	SELECT rid.EndStationId
		, des.Name
		, des.Latitude
		, des.Longitude
		, COUNT(1) AS Arrivals
	FROM dbo.rides rid
		JOIN stations des ON rid.EndStationId = des.Id
	GROUP BY rid.EndStationId
	, des.Name
	, des.Latitude
	, des.Longitude
	) info
ORDER BY Arrivals desc