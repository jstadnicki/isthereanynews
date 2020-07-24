//resource "azurerm_mssql_server" "itan-mssql-server" {
//  administrator_login = ""
//  administrator_login_password = ""
//  location = var.location
//  name = "itan-mssql-server"
//  resource_group_name = var.resource-group-name
//  version = "12.0"
//  
//}
//
//resource "azurerm_mssql_database" "itan-mssql-database" {
//  name = "itan"
//  server_id = azurerm_mssql_server.itan-mssql-server.id
//  sku_name = "basic"
//}