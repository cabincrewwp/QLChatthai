select n.Code_KH,dm.Ten_CT,dm.Ma_CT,ct.dvt,ct.trangthai,xl.phuongphap,dm.tuxuly,ct.slthuc
	from nhap n
	join Khachhang kh on kh.Code_KH =n.Code_KH 
	join CTNHap ct on ct.sophieu=n.sophieu
	join DM_CT dm on dm.idct =ct.idct
	join Xuly xl on xl.idxl=ct.idxl 
	where n.ngaynhan>='2017-01-01' and n.ngaynhan<='2017-03-31' and ct.slthuc is null
