# UPS_Hibernate
通过ping不在UPS上的网络设备来判定是否停电,使Windows转入待机状态.  
虽然叫hibernate,但实际执行的是Suspend.  
每60秒检测一次,失败后延迟120秒再次检测,两次失败后才会待机,并且可以手动取消.  
