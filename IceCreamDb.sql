U S E   [ m a s t e r ]  
 G O  
 C R E A T E   D A T A B A S E   [ I c e C r e a m D b ]   O N     P R I M A R Y    
 G O  
 U S E   [ I c e C r e a m D b ]  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ h i b e r n a t e _ u n i q u e _ k e y ] (  
 	 [ n e x t _ h i ]   [ i n t ]   N O T   N U L L  
 )   O N   [ P R I M A R Y ]  
 G O  
 C R E A T E   T A B L E   [ d b o ] . [ P r o d u c t s ] (  
 	 [ P r o d u c t I d ]   [ i n t ]   I D E N T I T Y ( 1 , 1 )   N O T   N U L L ,  
 	 [ N a m e ]   [ n v a r c h a r ] ( 1 0 0 )   N U L L  
 )   O N   [ P R I M A R Y ]  
 G O  
 