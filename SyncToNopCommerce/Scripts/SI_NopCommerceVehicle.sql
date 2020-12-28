create procedure SI_NopCommerceVehicle @LastVehicleSync datetime
 as
 begin

SELECT T1.U_Make[Make],T1.U_Model[ModelNo],T1.U_ModelName[ModelName],
T1.U_VehicleChassisGrp[VehicleChassisGrp],T1.U_Chassis[Chassis],T1.U_VehicleEngineGrp[VehicleEngineGrp] ,T1.U_Engine[Engine],T1.U_CCPrefix[CCPrefix],
T1.U_CC[CC],T1.U_CCSuffix[CCSufix],T1.U_HandDrive[HandDrive],T1.U_Transmission[TransmissionType],T1.U_TransmissionCode[TransmissionCode],T1.U_FuelType[FuelType],T1.U_Manufacture[CountryOfManufacture],T1.U_ManufStart[ManufactureStart],
T1.U_ManufEnd[ManufactureEnd],T1.U_LastModifiedBy[LastModifiedBy],T1.U_LastModifiedDateTime[LastModifiedDate],T1.U_VehicleID[SAPVehicleId]
FROM [@si_vehicle] T1   where T1.U_ManufEnd is not null and (T1.U_CreatedDateTime
> @LastVehicleSync or T1.U_LastModifiedDateTime 
> @LastVehicleSync )
end


create procedure SI_SaveNopCommerceVehicle
(
    @Make nvarchar(50),
    @ModelNo nvarchar(100),
    @ModelName nvarchar(50),
    @VehicleChassisGrp varchar(150),
    @Chassis nvarchar(50),
    @VehicleEngineGrp nvarchar(100),
    @Engine nvarchar(50),
    @CCPrefix nvarchar(100),
    @CC nvarchar(100),
    @CCSufix nvarchar(100),
    @HandDrive nvarchar(10),
    @TransmissionType nvarchar(50),
	@TransmissionCode nvarchar(50),
    @FuelType nvarchar(100),
    @CountryOfManufacture nvarchar(50),
    @ManufactureStart datetime,
    @ManufactureEnd datetime,
    @LastModifiedBy nvarchar(50),
	@LastModifiedDate datetime,
	@SAPVehicleId int
)
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Nop.dbo.Vehicle
        WHERE SAPVehicleId=@SAPVehicleId
            
    )

    BEGIN
        UPDATE Nop.dbo.Vehicle set Make= @Make, ModelNo=@ModelNo,ModelName=@ModelName,VehicleChassisGrp=@VehicleChassisGrp,
		 Chassis= @Chassis,VehicleEngineGrp=@VehicleEngineGrp, Engine= @Engine,CCPrefix=@CCPrefix, CC= @CC,  CCSufix=  @CCSufix,
		 HandDrive=  @HandDrive, TransmissionType= @TransmissionType,TransmissionCode=@TransmissionCode,FuelType=@FuelType,
		 CountryOfManufacture= @CountryOfManufacture, ManufactureStart= @ManufactureStart,ManufactureEnd=@ManufactureEnd,
		 LastModifiedBy=@LastModifiedBy,LastModifiedDate=@LastModifiedDate
         WHERE SAPVehicleId=@SAPVehicleId
    END
ELSE
    BEGIN
        INSERT into Nop.dbo.Vehicle (Make,ModelNo,ModelName,VehicleChassisGrp,
		 Chassis,VehicleEngineGrp,Engine,CCPrefix, CC, CCSufix,
		 HandDrive, TransmissionType,TransmissionCode,FuelType,
		 CountryOfManufacture,ManufactureStart,ManufactureEnd,
		 LastModifiedBy,LastModifiedDate,SAPVehicleId) Values ( @Make,@ModelNo,@ModelName,@VehicleChassisGrp,
		 @Chassis,@VehicleEngineGrp,@Engine,@CCPrefix, @CC, @CCSufix,
		 @HandDrive, @TransmissionType,@TransmissionCode,@FuelType,
		 @CountryOfManufacture,@ManufactureStart,@ManufactureEnd,
		 @LastModifiedBy,@LastModifiedDate,@SAPVehicleId)
    END

END