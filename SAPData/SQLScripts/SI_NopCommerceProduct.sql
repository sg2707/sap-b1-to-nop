
 create procedure SI_NopCommerceProduct 
 as

 begin
 select top 10 Product.ItemCode [sku], ItemName [name]
, [U_SI_PartNumber] [short_description]
, UserText [full_description] ,(select Id from Nop.dbo.Manufacturer where Name COLLATE DATABASE_DEFAULT=U_SI_Brand COLLATE DATABASE_DEFAULT) [manufacturer_ids],
(select Id from Nop.dbo.Category where MetaKeywords COLLATE DATABASE_DEFAULT=Grp.U_SI_MainGroup)  [CategoryId]
, 10.00 [price], cast((Stock.OnHand - Stock.IsCommited)as int) as stock_quantity
from OITM [Product]
inner join [OITW] Stock on Product.ItemCode = Stock.ItemCode and Stock.WhsCode = '01'
inner join [OITB] Grp on Grp.ItmsGrpCod = Product.ItmsGrpCod
where frozenFor = 'N' and  Grp.U_SI_MainGroup is not null and ( (cast(concat(Convert(date,Product.CreateDate),
' ', convert(char(8), dateadd(second, Product.createts, '') , 114)) as datetime))
> (select max(CreatedOnUtc) from Nop.dbo.Product) or (cast(concat(Convert(date,Product.UpdateDate),
' ', convert(char(8), dateadd(second, Product.UpdateTS, '') , 114)) as datetime))
> (select max(UpdatedOnUtc) from Nop.dbo.Product))

end
