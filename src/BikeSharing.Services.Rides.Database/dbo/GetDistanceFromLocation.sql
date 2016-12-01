CREATE FUNCTION [dbo].[GetDistanceFromLocation]
(   
    @CurrentLatitude float,
    @CurrentLongitude float,
    @latitude float,
    @longitude float
)
RETURNS int
AS
BEGIN
    DECLARE @geo1 geography = geography::Point(@CurrentLatitude, @CurrentLongitude, 4268), 
            @geo2 geography = geography::Point(@latitude, @longitude, 4268)

    DECLARE @distance int
    SELECT @distance = @geo1.STDistance(@geo2) 

    RETURN @distance

END