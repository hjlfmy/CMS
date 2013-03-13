﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Bootstrap.Extensions.StartupTasks;
using YangKai.BlogEngine.Infrastructure;
using YangKai.BlogEngine.Modules.BoardModule.Objects;
using YangKai.BlogEngine.Modules.CommonModule.Objects;
using YangKai.BlogEngine.Modules.PostModule.Objects;

namespace YangKai.BlogEngine.Web.Mvc.BootStrapper
{
    public class DataInitializeConfig : IStartupTask
    {
        public void Run()
        {
            using (var context = new BlogEngineContext())
            {
                if (!context.Database.Exists())
                {
                    //初始化数据
                    var user = CreateUser();
                    var channel = CreateChannel();
                    var group = CreateGroup(channel);
                    var post = CreatePost(user, group);
                    var comment = CreateComment(post);
                    var board = CreateBoard();

                    //保存数据
                    context.Set<User>().Add(user);
                    context.Set<Channel>().Add(channel);
                    context.Set<Group>().Add(group);
                    context.Set<Post>().Add(post);
                    context.Set<Comment>().Add(comment);
                    context.Set<Board>().Add(board);
                    context.SaveChanges();
                }
            }
        }

        private static User CreateUser()
        {
            return new User()
                {
                    UserName = "管理员",
                    LoginName = "admin",
                    Password = "123",
                    Email="test@test.com",
                };
        }

        private static Channel CreateChannel()
        {
            return new Channel()
                {
                    Name = "channel",
                    Url = "channel",
                    Description = "这是一个默认频道.",
                    IsDefault = true,
                    OrderId = 1,
                    BannerUrl = "/Content/Image/banner.png",
                };
        }

        private static Group CreateGroup(Channel channel)
        {
            return new Group()
                {
                    Name = "group",
                    Url = "group",
                    Description = "这是一个默认分组.",
                    OrderId = 1,
                    Channel = channel,
                };
        }

        private static Post CreatePost(User user, Group group)
        {
            return new Post()
                {
                    Url = "test",
                    Title = "测试文章",
                    Description = "<p>这是文章的描述内容.</p>",
                    PubAdmin = user,
                    Group = group,
                    Tags = new List<Tag> {new Tag {Name = "Test Tag"}},
                    Categorys = new List<Category>
                        {
                            new Category
                                {
                                    Name = "default",
                                    Url = "default",
                                    Description = "这是一个默认分类.",
                                    OrderId = 1,
                                    Group = group,
                                }
                        },
                    Pages = new List<Page>
                        {
                            new Page
                                {
                                    Title = string.Empty,
                                    Content = "<p>这是文章的正文内容.</p>",
                                    OrderId = 1,
                                }
                        },
                };
        }

        private static Comment CreateComment(Post post)
        {
            return new Comment
                {
                    Content = "<p>这是评论的内容.</p>",
                    Author="guest",
                };
        }

        private static Board CreateBoard()
        {
            return new Board
            {
                Content = "<p>这是留言的内容.</p>",
                Author = "guest",
            };
        }

        public void Reset()
        {
        }
    }
}