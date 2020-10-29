create procedure SI_NopCommerceCategory 
 as
 begin
	Select u.FldValue [meta_keywords],u.Descr [name] FROM UFD1 as u join CUFD as c ON c.TableID = u.TableID 
                      WHERE(c.FieldID = u.FieldID) AND (c.TableID = 'OITB') AND c.AliasID='SI_MainGroup' 

end
