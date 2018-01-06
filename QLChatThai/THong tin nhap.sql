select nhap.Code_KH,Khachhang.Ten_KH,DM_CT.Ma_CT,DM_CT.Ten_CT,DM_CT.tuxuly,ctnhap.idct,ctnhap.slthuc
from nhap join 
Khachhang on nhap.Code_KH =Khachhang.Code_KH
join CTNHap on nhap.sophieu=CTNHap.Sophieu join dm_ct on ctnhap.idct=DM_CT.idct