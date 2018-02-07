﻿using System;
using System.Collections.Generic;
using SiteServer.Plugin;
using SS.Login.Api;
using SS.Login.Core;
using SS.Login.Pages;
using SS.Login.Parse;
using SS.Login.Provider;

namespace SS.Login
{
    public class Main : PluginBase
    {
        public static Main Instance { get; private set; }

        public override void Startup(IService service)
        {
            service
                .AddDatabaseTable(OAuthDao.TableName, OAuthDao.Columns)
                .AddStlElementParser(StlLogin.ElementName, StlLogin.Parse)
                .AddStlElementParser(StlLogout.ElementName, StlLogout.Parse)
                .AddStlElementParser(StlRegister.ElementName, StlRegister.Parse)
                .AddPluginMenu(new Menu
                {
                    Text = "账号授权认证",
                    Menus = new List<Menu>
                    {
                        new Menu
                        {
                            Text = "授权认证设置",
                            Href = $"{nameof(PageSettings)}.aspx"
                        },
                        new Menu
                        {
                            Text = "第三方登录设置",
                            Href = $"{nameof(PageOAuth)}.aspx"
                        }
                    }
                })
                ;

            service.ApiGet += Service_ApiGet;
            service.ApiPost += Service_ApiPost;

            Instance = this;
        }

        private object Service_ApiGet(object sender, ApiEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Action) && !string.IsNullOrEmpty(args.Id))
            {
                if (Utils.EqualsIgnoreCase(args.Action, nameof(StlLogin.OAuth)))
                {
                    return StlLogin.OAuth(args.Request, args.Id);
                }
                if (Utils.EqualsIgnoreCase(args.Action, nameof(StlLogin.OAuthRedirect)))
                {
                    return StlLogin.OAuthRedirect(args.Request, args.Id);
                }
                if (Utils.EqualsIgnoreCase(args.Action, nameof(ApiHttpGet.Captcha)))
                {
                    return ApiHttpGet.Captcha(args.Id);
                }
            }

            throw new Exception("请求的资源不在服务器上");
        }

        private static object Service_ApiPost(object sender, ApiEventArgs args)
        {
            if (string.IsNullOrEmpty(args.Action) || !Utils.EqualsIgnoreCase(args.Action, "actions") ||
                string.IsNullOrEmpty(args.Id)) throw new Exception("请求的资源不在服务器上");

            var request = args.Request;
            var id = args.Id;

            if (Utils.EqualsIgnoreCase(id, nameof(StlRegister.Register)))
            {
                return StlRegister.Register(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(StlLogin.Login)))
            {
                return StlLogin.Login(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(StlLogout.Logout)))
            {
                return StlLogout.Logout(request);
            }

            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.LoadConfig)))
            {
                return ApiJsonActionsPost.LoadConfig(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.ResetPassword)))
            {
                return ApiJsonActionsPost.ResetPassword(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.ResetPasswordByToken)))
            {
                return ApiJsonActionsPost.ResetPasswordByToken(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.Edit)))
            {
                return ApiJsonActionsPost.Edit(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.IsMobileExists)))
            {
                return ApiJsonActionsPost.IsMobileExists(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.IsPasswordCorrect)))
            {
                return ApiJsonActionsPost.IsPasswordCorrect(request);
            }
            if (Utils.EqualsIgnoreCase(id, nameof(ApiJsonActionsPost.IsCodeCorrect)))
            {
                return ApiJsonActionsPost.IsCodeCorrect(request);
            }

            throw new Exception("请求的资源不在服务器上");
        }
    }
}