CREATE PROCEDURE [dbo].[pStationsNear]
	@latitude float,
	@longitude float,
	@size int = 5000
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	SELECT TOP( @size) station.*, j.Occupied, dbo.[GetDistanceFromLocation](station.Latitude, station.Longitude, @latitude, @longitude) as Distance 
	FROM stations AS station
	LEFT OUTER JOIN (SELECT s.id, COUNT(b.id) AS Occupied FROM stations s LEFT OUTER JOIN bikes as b ON b.stationId = s.id GROUP BY s.id) AS j ON j.id = station.id
	ORDER BY dbo.[GetDistanceFromLocation](station.Latitude, station.Longitude, @latitude, @longitude)
END
GO