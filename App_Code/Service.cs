using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data.OracleClient;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string DoPicking(string productCode, 
        string providerLote, 
        int pickingQuntity, 
        string warehouseCode, 
        string clientName,
        string shipNumber,
        string userCode,
        string username, 
        string password)
    {
        OracleConnection conn = null;
        string sql = "insert into sga_littmoden.custom_sivart_picking " +
            "(cod_prod, lote_proveedor, cantidad_picking, cod_alm, creation_date, nombre_cliente, codigo_pedido, codigo_operario) " +
            "values ('" + productCode + "', '" + providerLote + "', " + pickingQuntity + ", '" + warehouseCode + "', SYSDATE, '" + clientName + "', '" + shipNumber + "', '" + userCode + "')";
        string sql2 = "insert into sga_littmoden.custom_sivart_picking " +
            "(cod_prod, lote_proveedor, cantidad_picking, cod_alm, creation_date) " +
            "values ('" + productCode + "', '" + providerLote + "', " + pickingQuntity + ", '" + warehouseCode + "', SYSDATE)";
        string result = "OK";

        try
        {
            conn = new OracleConnection("Data Source=orclmlx; User Id=mlxsga_protein_sivart; Password=mlxsga_protein_sivart;");
            conn.Open();
            OracleCommand cmd = new OracleCommand(sql2, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            result = "Exception, " + sql2 + " " + ex.ToString();
        }
        finally
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return result;
    }


    [WebMethod]
    public string ValidateWarehouseCode(string warehouseCode, string username, string password)
    {
        OracleConnection conn = null;
        string sql = "select count(*) from sga_littmoden.wms_warehouse w where w.warehouse_code= '" + warehouseCode + "'";
        string result = "OK";

        try
        {
            conn = new OracleConnection("Data Source=orclmlx; User Id=mlxsga_protein_sivart; Password=mlxsga_protein_sivart;");
            conn.Open();
            OracleCommand cmd = new OracleCommand(sql, conn);
            object res = cmd.ExecuteScalar();
            if (res != null)
            {
                if (Convert.ToInt32(res) > 0)
                {
                    //OK
                }
                else
                {
                    result = "Error, warehouse code not found. Found " + res.ToString() + " results.";
                }
            }
            else
            {
                result = "Error, in query (" + sql + ")";
            }
            cmd.Dispose();
        }
        catch (Exception ex)
        {
            result = "Exception, " + sql + " " + ex.ToString();
        }
        finally
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }

        return result;
    }
 
}
