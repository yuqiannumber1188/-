using System;
using System.Data;
using System.Data.SqlClient;
using cjpc.utils.dbutils;

namespace cjpc.utils.menber
{
    class Teacher
    {
         DBClass dbobj = new DBClass();
         public Teacher()
        {
        //
        // TODO: 在此处添加构造函数逻辑
        //
        }

    /// <summary>
    /// 验证用户登陆
    /// </summary>
    /// <param name="MemberCode">账号</param>
    /// <param name="Password">密码</param>
    /// <returns></returns>
    public object Login(object MemberCode, object Password)
    {
        string sqlstr = "Select TEACHER_ID from TEACHER_INFO where TEACHER_CODE=@MemberCode and PSW=@Password";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberCode = new SqlParameter("@MemberCode", SqlDbType.NVarChar, 50);
        myMemberCode.Value = MemberCode;
        myCmd.Parameters.Add(myMemberCode);
        SqlParameter myPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        myPassword.Value = Password;
        myCmd.Parameters.Add(myPassword);
        return dbobj.ExecScalar(myCmd);
    }

    public void Delete(object MemberCode)
    {
        string sqlstr = "delete from TEACHER_INFO where TEACHER_CODE=@MemberCode";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberCode = new SqlParameter("@MemberCode", SqlDbType.NVarChar, 50);
        myMemberCode.Value = MemberCode;
        myCmd.Parameters.Add(myMemberCode);
        dbobj.ExecNonQuery(myCmd);
    }

    public void Add(object employeecode, object employeename, object employeeorgcode, object employeerole, object employeepsw)
    {
        string sqlstr = "insert into TEACHER_INFO ([TEACHER_CODE],[TEACHER_NAME],[ORG_CODE],[ROLE_CODE],[PSW]) values(@employeecode,@employeename,@employeeorgcode,@employeerole, @employeepsw)";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);

        SqlParameter[] oParms = {
                                        new SqlParameter("@employeecode",SqlDbType.NVarChar,50),
                                        new SqlParameter("@employeename",SqlDbType.NVarChar,50),
                                        new SqlParameter("@employeeorgcode",SqlDbType.NVarChar,50),
                                        new SqlParameter("@employeerole",SqlDbType.NVarChar,50),
                                        new SqlParameter("@employeepsw",SqlDbType.VarChar,50),                                       
                                    };
        oParms[0].Value = employeecode;
        oParms[1].Value = employeename;
        oParms[2].Value = employeeorgcode;
        oParms[3].Value = employeerole;
        oParms[4].Value = employeepsw;
       
        foreach (SqlParameter parmater in oParms)
        {
            myCmd.Parameters.Add(parmater);
        }
        dbobj.ExecNonQuery(myCmd);
    }



    /// <summary>
    /// 得到用户详细信息
    /// </summary>
    /// <param name="MemberCode">用户Code</param>
    /// <returns></returns>

    public DataRow Info(object MemberCode)
    {
        string sqlstr = "select * from Employee_info where employee_code=@MemberCode";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberCode = new SqlParameter("@MemberCode", SqlDbType.NVarChar, 50);
        myMemberCode.Value = MemberCode;
        myCmd.Parameters.Add(myMemberCode);
        dbobj.ExecNonQuery(myCmd);
        //DataRow mydr = dbobj.GetDataSet(myCmd,"Member").Tables[0].Rows[0];
        DataSet myds = dbobj.GetDataSet(myCmd, "Member");
        if (myds.Tables[0].Rows.Count > 0)
            return myds.Tables[0].Rows[0];
        else
            return null;
    }

    /// <summary>
    /// 得到用户的角色
    /// </summary>
    /// <param name="MemberCode">用户Code</param>
    /// <returns></returns>
    public object GetRole(object MemberCode)
    {
        DataRow role = Info(MemberCode);
        if (role == null)
        {
            return null;
        }
        else
        {
            return role["ROLE_CODE"];
        }
    }

    /// <summary>
    /// 重设密码
    /// </summary>
    /// <param name="MemberCode">会员编号</param>
    /// <param name="Password">密码</param>

    public void UpdatePassword(object MemberCode, object Password)
    {
        string sqlstr = "update TEACHER_INFO set PSW=@Password where TEACHER_CODE=@MemberCode";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberCode = new SqlParameter("@MemberCode", SqlDbType.NVarChar, 50);
        myMemberCode.Value = MemberCode;
        myCmd.Parameters.Add(myMemberCode);
        SqlParameter myPassword = new SqlParameter("@Password", SqlDbType.VarChar, 50);
        myPassword.Value = Password;
        myCmd.Parameters.Add(myPassword);
        dbobj.ExecNonQuery(myCmd);
    }

    public void UpdateORG(object MemberCode, object OrgCode)
    {
        string sqlstr = "update TEACHER_INFO set ORG_CODE=@OrgCode where TEACHER_CODE=@MemberCode";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberCode = new SqlParameter("@MemberCode", SqlDbType.NVarChar, 50);
        myMemberCode.Value = MemberCode;
        myCmd.Parameters.Add(myMemberCode);
        SqlParameter myORG = new SqlParameter("@OrgCode", SqlDbType.NVarChar, 50);
        myORG.Value = OrgCode;
        myCmd.Parameters.Add(myORG);
        dbobj.ExecNonQuery(myCmd);
    }

    public bool HadMember(object MemberName)
    {
        string sqlstr = "select Count(TEACHER_NAME) from TEACHER_INFO where TEACHER_NAME = @MemberName";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        SqlParameter myMemberName = new SqlParameter("@MemberName", SqlDbType.NVarChar, 50);
        myMemberName.Value = MemberName;
        myCmd.Parameters.Add(myMemberName);
        return Convert.ToBoolean(dbobj.ExecScalar(myCmd));
    }

    /// <summary>
    /// 得到DataSet
    /// </summary>
    /// <param name="emprole">用户名和code</param>
    /// <returns></returns>
    public DataSet GetNameAndCode()
    {
        string sqlstr = "select TEACHER_CODE AS nac,TEACHER_NAME from TEACHER_INFO where TEACHER_CODE != 'admin'";
        SqlCommand myCmd = dbobj.GetCommandStr(sqlstr);
        dbobj.ExecNonQuery(myCmd);
        DataSet myds = dbobj.GetDataSet(myCmd, "empTalbe");
        return myds;
    }

    }
}
