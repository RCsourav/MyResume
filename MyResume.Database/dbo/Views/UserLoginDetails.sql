

CREATE view [dbo].[UserLoginDetails] as
select 
	ulh.Id as UserHistoryId,
	uip.Id as IpAddressId,
ud.Id as UserId,
	ud.UserName,
	ud.EmailId,
	uip.IpAddress,
	ulh.LoginTime,
	ulh.LogoutTime,
	ulh.IsActive,
	ulh.LastActivityTime
	from UserLoginHistory ulh
	inner join UserDetails ud on ulh.UserId = ud.Id
	inner join UserIpAddress uip on ulh.IpAddressId = uip.Id;

