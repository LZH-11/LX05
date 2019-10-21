using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace DAL
{
  public   class Activity
    {
        private Activity() { }
        private static Activity _instance = new Activity();
        public static Activity Instance
        {
            get { return _instance; }
        }
        string cns = AppConfigurtaionServices.Configuration.GetConnectionString("cns");
        public Model.Activity GetModel(int id)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select activityId,activityName,endTime,activityPicture,summary,activityVerify,userName,recommend,recommendtime,activityIntroduction,endTime>=now() as activityStatus from activity where activityId=@id";
                return cn.QueryFirstOrDefault<Model.Activity>(sql, new { id = id });
            }
        }

        public int GetverifyCount()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select count(1) from activity where activityVerify='审核通过'";
                return cn.ExecuteScalar<int>(sql);
            }
        }
        public IEnumerable<Model.Activity> GetVerifyPage(Model.Page page)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "with a as(select row_number() over(order by endTime desc) as num, activityId,activityName,endTime,activityPicture,summary,activityVerify,userName,recommend,recommendtime,endTime>=now() as activityStatus from activity where activityVerify='审核通过')";
                sql += "select * from a where num between (@pageIndex-1)*@pageSize+1 and @pageIdnex*@pageSize";
                return cn.Query<Model.Activity>(sql, page);
            }
        }

        public IEnumerable<Model.Activity> GetNew()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select * from activity where activityVerify='审核通过' and endTime>=now()";
                return cn.Query<Model.Activity>(sql);
            }
        }

        public Model.Activity GetRecommend()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select * from activity where activityVerify='审核通过' and recommend='是'";
                return cn.QueryFirstOrDefault<Model.Activity>(sql);
            }
        }
        //2222222222222222222222222222
        //返回最新的1个已过期活动
        public Model.Activity GetEnd()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select * from activity where activityVerify='审核通过' and endTime<now() order by endTime desc limit 1";
                return cn.QueryFirstOrDefault<Model.Activity>(sql);
            }
        }

        //返回所有活动的id和名称
        public IEnumerable<Model.Activity> GetActivityNames()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select activityid,activityName from activity";
                return cn.Query<Model.Activity>(sql);
            }
        }

        public int  GetCount()
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "select count(1) from activity";
                return cn.ExecuteScalar<int>(sql);
            }
        }

        public IEnumerable<Model.Activity> GetPage(Model.Page page)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "with a as(select row_number() over(order by endTime desc) as num,activity.* from activity)";
                sql += "select * from a where num between(@pageIndex-1)*@pageSize+1 and @pageIndex*@pageSize";
                return cn.Query<Model.Activity>(sql,page);
            }
        }

        //33333333
        public int Add(Model.Activity active)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "insert into activity(activityname,endtime,activitypicture,activityintroduction,summary,activityverify,activitystatus,username,recommend) values(@activityName, @endTime, @activityPicture, @activityIntroduction, @summary, @activityVerify, @activityStatus, @userName, @recommend);";
                sql += "select @@identity";
                return cn.Execute(sql,active);
            }
        }

        public int Update(Model.Activity active)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "update activity set activityname=@activityName, endtime=@endTime, activitypicture=@activityPicture, activityintroduction=@activityIntroduction, summary=@summary, activityverify=@activityVerify, activitystatus=@activityStatus where activityid=@activityId";
                return cn.Execute(sql, active);
            }
        }

        public int UpdateImg(Model.Activity active)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "update activity set activitypicture=@activityPicture where activityid=@activityId";
                return cn.Execute(sql, active);
            }
        }

        public int UpadaeVerify(Model.Activity active)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "update activity set activityVerify=@activityVerify where activityid=@activityId";
                return cn.Execute(sql, active);
            }
        }

        //44444444444444444444444444444444
        public int Delete(int id)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "delete from activity where activityid=@id";
                return cn.Execute(sql, new { id=id});
            }
        }

        public int updateRecommend(Model.Activity activity)
        {
            using (IDbConnection cn = new MySqlConnection(cns))
            {
                string sql = "update activity set recommend=@recommend,recommendTime where activityid=@activityid";
                return cn.Execute(sql, activity);
            }
        }

    }
}
