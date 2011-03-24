' Copyright (c) Microsoft Corporation.  All rights reserved.

Imports System
Imports System.Text
Imports System.Collections.Generic
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports NorthwindEF

<TestClass()> Public Class QueryTests

    <TestMethod()> Public Sub VBAnonymousTypeInQuery()
        Using context As New NorthwindEntities
            Dim a = From c In context.Customers Where c.CustomerID = "ALFKI" Select New With {.A = c.CompanyName, .Cust = c}
            Dim result = a.ToArray()
            Assert.AreEqual(1, result.Length)
            Assert.AreEqual("Alfreds Futterkiste", result(0).A)
            Assert.AreEqual("Alfreds Futterkiste", result(0).Cust.CompanyName)
        End Using
    End Sub
End Class
