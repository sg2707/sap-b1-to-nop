create procedure SI_NopCommerceManufacturer @LastManufacturSync datetime
 as
 begin
select U_SI_Brand [name],max((cast(concat(Convert(date,CreateDate),
' ', convert(char(8), dateadd(second, createts, '') , 114)) as datetime))) [CreatedDate],
 max((cast(concat(Convert(date,UpdateDate),
' ', convert(char(8), dateadd(second,UpdateTS, '') , 114)) as datetime))) [UpdatedDate]
from OITM group by U_SI_Brand having(
max((cast(concat(Convert(date,CreateDate),
' ', convert(char(8), dateadd(second, createts, '') , 114)) as datetime)))
> @LastManufacturSync or max((cast(concat(Convert(date,UpdateDate),
' ', convert(char(8), dateadd(second,UpdateTS, '') , 114)) as datetime)))
> @LastManufacturSync) and U_SI_Brand is not null
end
