Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "        AUTOMATED API TEST SUITE - PRN232 ASSIGNMENT 1     " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan

$baseUrl = "http://localhost:5190/api"
$passed = 0
$failed = 0

function Run-Test {
    param(
        [string]$TestName,
        [scriptblock]$Action
    )
    Write-Host -NoNewline "[RUNNING] $TestName ... "
    try {
        & $Action
        Write-Host "PASS" -ForegroundColor Green
        $script:passed++
    } catch {
        Write-Host "FAIL: $($_.Exception.Message)" -ForegroundColor Red
        $script:failed++
    }
}

# 1. Test Public Active Articles
Run-Test "1. Public: Get Active Articles (No Auth required)" {
    $res = Invoke-RestMethod -Uri "$baseUrl/NewsArticles/active" -Method Get
    if ($res.Count -lt 1) { throw "No articles returned" }
}

# 2. Test Admin Login
Run-Test "2. Auth: Admin Login (from appsettings.json)" {
    $body = @{ email = "admin@FUNewsManagementSystem.org"; password = "@@abc123@@" } | ConvertTo-Json
    $res = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method Post -Body $body -ContentType "application/json"
    if ($res.Role -ne "Admin") { throw "Expected role 'Admin', got '$($res.Role)'" }
}

# 3. Test Staff Login
Run-Test "3. Auth: Staff Login (from database)" {
    $body = @{ email = "IsabellaDavid@FUNewsManagement.org"; password = "@1" } | ConvertTo-Json
    $res = Invoke-RestMethod -Uri "$baseUrl/Auth/login" -Method Post -Body $body -ContentType "application/json"
    if ($res.Role -ne "Staff") { throw "Expected role 'Staff', got '$($res.Role)'" }
}

# 4. Test Categories List
Run-Test "4. Categories: Get all categories" {
    $res = Invoke-RestMethod -Uri "$baseUrl/Categories" -Method Get
    if ($res.Count -lt 1) { throw "No categories returned" }
}

# 5. Test Delete Category Constraint (Should fail with 400 because Category 1 has articles)
Run-Test "5. Constraint: Cannot delete Category assigned to articles (Expect 400)" {
    try {
        $null = Invoke-RestMethod -Uri "$baseUrl/Categories/1" -Method Delete
        throw "Expected 400 Bad Request but request succeeded!"
    } catch {
        if ($_.Exception.Message -match "400") {
            # Expected behavior
        } else {
            throw $_
        }
    }
}

# 6. Test Delete Account Constraint (Should fail with 400 because Account 1 created articles)
Run-Test "6. Constraint: Cannot delete Account that created articles (Expect 400)" {
    try {
        $null = Invoke-RestMethod -Uri "$baseUrl/Accounts/1" -Method Delete
        throw "Expected 400 Bad Request but request succeeded!"
    } catch {
        if ($_.Exception.Message -match "400") {
            # Expected behavior
        } else {
            throw $_
        }
    }
}

# 7. Test Admin Report
Run-Test "7. Report: Filter articles between dates sorted descending" {
    $res = Invoke-RestMethod -Uri "$baseUrl/Reports?startDate=2024-01-01&endDate=2025-01-01" -Method Get
    if ($res.Count -lt 1) { throw "No report records returned" }
}

# 8. Test OData Query
Run-Test "8. OData: Filter articles with CategoryId eq 1" {
    $res = Invoke-RestMethod -Uri "$baseUrl/NewsArticles?`$filter=CategoryId eq 1" -Method Get
    if ($res.Count -lt 1) { throw "No OData filtered records returned" }
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "SUMMARY: Total: $($passed + $failed) | Passed: $passed | Failed: $failed" -ForegroundColor $(if ($failed -eq 0) { "Green" } else { "Red" })
Write-Host "==========================================================" -ForegroundColor Cyan
