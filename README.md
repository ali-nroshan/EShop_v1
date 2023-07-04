# EShop
First version of EShop Projects

به اولین بخش از سری پروژه های متن باز فروشگاه اینترنتی خوش آمدید !

این سری پروژه ها شامل حداقل 4 بخش میباشد که در هر قسمت ما سعی در ارتقای موارد زیر داریم :
 
1- توسعه و تکمیل امکانات فروشگاه.

2- بهینه سازی - افزایش مقیاس پذیری پروژه.

3-استفاده از ابزار و فناوری های جدید.

4- تغییر معماری پروژه در صورت نیاز جهت مدیریت هرچه بهتر پیچیدگی ها.

5- تهیه مستندات بهتر :)

-------------------------------------------------


پروژه EShop_v1 از پروژه EShop_v0 الهام گرفته شده که با ASP.Net Core 6 MVC از صفر توسط خودم ساخته شده بود، در این ورژن تغییرات زیر اعمال شده است : 

1- تغییر معماری پروژه به Clean Architecture.

2- ارتقا به DotNet 7.

3- تکمیل / بهینه سازی / پیاده سازی بهتر.

 
 * نکته : بخش فرانت اند در سورس حذف شده است زیرا از قالب آماده استفاده شده که اجازه انتشار سورسش رو ندارم.

-------------------------------------------------

پروژه EShop_v1 دارای 6 بخش میباشد : 

1- لایه Presentation شامل پروژه WebApp (ASP.Net Core 7 MVC)

2- لایه Infrastructure شامل پروژه EShop.Infrastructure (.Net 7 Lib)

3- لایه Application/Core شامل پروژه EShop.Core (.Net 7 Lib)

4- لایه Domain شامل پروژه EShop.Domain (.Net 7 Lib)


تکنولوژی و ابزار های استفاده شده برای دیتابیس : 

1- ORM = EFCore ( Code First Approach )

2- Database = SqlServer2022

-------------------------------------------------

تشریح وابستگی لایه ها :‌

* Presentation Depends on Core/Infrastructure (All Layers)
* Infrastructure Depends on Core
* Core Depends on Domain
* Domain is core of project and has no dependencies
