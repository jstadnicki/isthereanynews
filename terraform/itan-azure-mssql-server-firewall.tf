resource "azurerm_sql_firewall_rule" "itan-mssql-server-firewall-rule" {
  start_ip_address = "${chomp(data.http.myip.body)}"
  end_ip_address = "${chomp(data.http.myip.body)}"
  name = "itan-tf-client-executer-ip"
  resource_group_name = var.resource-group-name
  server_name = azurerm_mssql_server.itan-mssql-server.name
  depends_on = [azurerm_mssql_server.itan-mssql-server]
}