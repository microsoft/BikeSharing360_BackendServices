CREATE VIEW [dbo].[vwVenueStations]
AS
SELECT VenueId
	, StationId
	, ROW_NUMBER () OVER (PARTITION BY VenueId ORDER BY GeoDistance) AS StationRanking
	FROM
	(
	SELECT ven.Id AS VenueId
		, sta.Id AS StationId
		, geography::STPointFromText('POINT(' + CAST(ven.Longitude AS VARCHAR(20)) + ' ' + CAST(ven.Latitude AS VARCHAR(20)) + ')', 4326).STDistance
			(
			geography::STPointFromText('POINT(' + CAST(sta.Longitude AS VARCHAR(20)) + ' ' + CAST(sta.Latitude AS VARCHAR(20)) + ')', 4326)
			) AS GeoDistance
	  FROM [dbo].[Venues] ven
	  JOIN [dbo].[stations] sta
		ON geography::STPointFromText('POINT(' + CAST(ven.Longitude AS VARCHAR(20)) + ' ' + CAST(ven.Latitude AS VARCHAR(20)) + ')', 4326).STDistance
			(
			geography::STPointFromText('POINT(' + CAST(sta.Longitude AS VARCHAR(20)) + ' ' + CAST(sta.Latitude AS VARCHAR(20)) + ')', 4326)
			)
			< 300
	) dis