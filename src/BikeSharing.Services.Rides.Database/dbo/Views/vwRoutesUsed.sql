
CREATE VIEW dbo.vwRoutesUsed AS
SELECT TOP 100 *
	, geography::STPointFromText('POINT(' + CAST(StartStationLongitude AS VARCHAR(20)) + ' ' + CAST(StartStationLatitude AS VARCHAR(20)) + ')', 4326) AS GeoPositionStart
	, geography::STPointFromText('POINT(' + CAST(EndStationLongitude AS VARCHAR(20)) + ' ' + CAST(EndStationLatitude AS VARCHAR(20)) + ')', 4326) AS GeoPositionEnd
FROM
	(
	SELECT rid.StartStationId
		, rid.EndStationId
		, ori.Name AS StartStationName
		, ori.Latitude AS StartStationLatitude
		, ori.Longitude AS StartStationLongitude
		, des.Name AS EndStationName
		, des.Latitude AS EndStationLatitude
		, des.Longitude AS EndStationLongitude
		, COUNT(1) AS RouteUsed
	FROM dbo.rides rid
		JOIN stations ori ON rid.StartStationId = ori.Id
		JOIN stations des ON rid.StartStationId = des.Id
	GROUP BY rid.StartStationId
	, rid.EndStationId
	, ori.Name
	, ori.Latitude
	, ori.Longitude
	, des.Name
	, des.Latitude
	, des.Longitude
	) info
WHERE StartStationId <> EndStationId
ORDER BY RouteUsed desc