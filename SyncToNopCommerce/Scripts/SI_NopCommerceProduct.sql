
 create procedure SI_NopCommerceProduct @LastProductSync datetime
 as

 begin
select Product.ItemCode [sku], U_SI_PartNumber [manufacturer_part_number],CodeBars [gtin]
,concat (U_SI_Brand,' ',Grp.U_SI_ItemClassDesc,U_SI_PartNumber) [name],
concat (U_SI_Brand,Grp.ItemClass) [short_description],isnull(U_SI_Length,0) [length]
,isnull(U_SI_Width,0) [width],isnull(U_SI_Height,0) [height],isnull(U_SI_Weight,0) [weight],
concat (U_SI_Brand,Grp.ItemClass) [full_description] ,U_SI_Brand [manufacturer],
Grp.U_SI_MainGroup  [category]
, 10.00 [price], cast((Stock.OnHand - Stock.IsCommited)as int) as stock_quantity
from OITM [Product]
inner join [OITW] Stock on Product.ItemCode = Stock.ItemCode and Stock.WhsCode = '01'
inner join [OITB] Grp on Grp.ItmsGrpCod = Product.ItmsGrpCod
where frozenFor = 'N' and  Grp.U_SI_MainGroup is not null and ( (cast(concat(Convert(date,Product.CreateDate),
' ', convert(char(8), dateadd(second, Product.createts, '') , 114)) as datetime))
> @LastProductSync or (cast(concat(Convert(date,Product.UpdateDate),
' ', convert(char(8), dateadd(second, Product.UpdateTS, '') , 114)) as datetime))
> @LastProductSync) 

end

