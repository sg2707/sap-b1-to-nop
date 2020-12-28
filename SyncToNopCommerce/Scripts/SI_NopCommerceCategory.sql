create PROCEDURE [dbo].[SI_NopCommerceCategory] 

AS 
  BEGIN 
      SELECT u.fldvalue [meta_keywords], 
             u.descr    [name],
			 null as  [parent_meta_keywords] 
      FROM   ufd1 AS u 
             JOIN cufd AS c 
               ON c.tableid = u.tableid 
      WHERE ( c.fieldid = u.fieldid ) 
            AND ( c.tableid = 'OITB' ) 
            AND c.aliasid = 'SI_MainGroup' 

	UNION ALL

      SELECT DISTINCT T0.u_si_subgroup + '_2' AS [meta_keywords], 
                      T0.u_si_subgroup [name], 
                      T1.meta_keywords AS [parent_meta_keywords] 
      FROM   oitb T0 
             INNER JOIN (SELECT u.fldvalue [meta_keywords], 
                                u.descr    [name] 
 FROM   ufd1 AS u 
                                JOIN cufd AS c 
                                  ON c.tableid = u.tableid 
                         WHERE ( c.fieldid = u.fieldid ) 
                               AND ( c.tableid = 'OITB' ) 
                               AND c.aliasid = 'SI_MainGroup') T1 
                     ON T0.u_si_maingroup = T1.meta_keywords 
      WHERE  Isnull(u_si_maingroup, '') <> '' 
             AND Isnull(u_si_subgroup, '') <> '' 
	
	UNION ALL

      SELECT DISTINCT T0.u_si_itemclassdesc + '_3' AS [meta_keywords], 
                      T0.u_si_itemclassdesc [name], 
                      T0.u_si_subgroup   +'_2'   AS parent_meta_keywords
					   FROM   oitb T0 
      WHERE  Isnull(u_si_itemclassdesc, '') <> '' 
             AND Isnull(u_si_subgroup, '') <> '' 
  END 