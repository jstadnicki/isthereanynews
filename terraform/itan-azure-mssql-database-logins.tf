resource "null_resource" "itan-mssql-logins-itan-reader" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database master -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query \"CREATE LOGIN [${var.itan-reader-mssql}] WITH PASSWORD='${random_password.itan-mssql-reader-password.result}'\""
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [azurerm_sql_firewall_rule.itan-mssql-server-firewall-rule, azurerm_mssql_database.itan-mssql-database]
}

resource "null_resource" "itan-mssql-logins-itan-writer" {
  provisioner "local-exec" {
    command = "Invoke-Sqlcmd -ServerInstance ${azurerm_mssql_server.itan-mssql-server.fully_qualified_domain_name} -Database master -Username ${var.itan-admin-mssql} -Password '${random_password.itan-mssql-admin-password.result}' -Query \"CREATE LOGIN [${var.itan-writer-mssql}] WITH PASSWORD='${random_password.itan-mssql-writer-password.result}'\""
    interpreter = ["PowerShell", "-Command"]
  }
  depends_on = [azurerm_sql_firewall_rule.itan-mssql-server-firewall-rule, azurerm_mssql_database.itan-mssql-database]
}