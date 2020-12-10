create procedure SI_NopCommerceManufacturer @LastManufacturSync datetime
 as
 begin
select U_SI_Brand [name]
from OITM group by U_SI_Brand having(
 U_SI_Brand is not null and(
 (max(CreateDate) > cast(@LastManufacturSync as DATE) or 
 (max(CreateDate) = cast(@LastManufacturSync as DATE) AND max(createts) >=  FORMAT(@LastManufacturSync,'HHmmss')))  or
 (max(UpdateDate)> cast(@LastManufacturSync as DATE) or 
 (max(UpdateDate) = cast(@LastManufacturSync as DATE) AND max(UpdateTS) >=  FORMAT(@LastManufacturSync,'HHmmss')))) )
end


