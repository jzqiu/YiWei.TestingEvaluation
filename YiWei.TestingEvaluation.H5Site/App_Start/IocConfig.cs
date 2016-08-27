using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YiWei.TestingEvaluation.H5Site
{
    using System.Reflection;
    using System.Web.Mvc;

    using Autofac;
    using Autofac.Integration.Mvc;

    public class IocConfig
    {
        public static void RegisterIocContainer()
        {
            var builder = new ContainerBuilder();
            var assembly = Assembly.GetExecutingAssembly();
            var repository = Assembly.Load("YiWei.TestingEvaluation.Data");
            builder.RegisterAssemblyTypes(repository, repository)
              .AsSelf();
            var service = Assembly.Load("YiWei.TestingEvaluation.Domain");
            builder.RegisterAssemblyTypes(service, service)
              .AsSelf();
            var weixin = Assembly.Load("YiWei.WeiXin.Domain");
            builder.RegisterAssemblyTypes(weixin, weixin)
              .AsSelf();
            builder.RegisterControllers(Assembly.Load("YiWei.TestingEvaluation.Controllers"));
            //容器
            var container = builder.Build();
            //注入改为Autofac注入
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}