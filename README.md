# UPS_Hibernate
使用UPS电池供电时使Windows转入待机状态,虽然叫hibernate,但实际执行的是Suspend.
可自定义设置不在UPS上的网络设备IP.
每60秒检测一次,失败后延迟120秒再次检测,两次失败后才会待机,并且可以手动取消.
