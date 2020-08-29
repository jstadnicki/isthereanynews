resource "azurerm_mssql_database" "itan-mssql-database" {
  name = "itan"
  server_id = azurerm_mssql_server.itan-mssql-server.id
  sku_name = "basic"
  max_size_gb = "2"

  depends_on = [
    azurerm_mssql_server.itan-mssql-server]
}