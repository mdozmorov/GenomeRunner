''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
' Copyright (c) Sergey Bochkanov (ALGLIB project).
' 
' >>> SOURCE LICENSE >>>
' This program is free software; you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation (www.fsf.org); either version 2 of the
' License, or (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' A copy of the GNU General Public License is available at
' http://www.fsf.org/licensing/licenses
' >>> END OF LICENSE >>>
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Module XAlglib

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'    Callback definitions for optimizers/fitters/solvers.
'    
'    Callbacks for unparameterized (general) functions:
'    * ndimensional_func         calculates f(arg), stores result to func
'    * ndimensional_grad         calculates func = f(arg), 
'                                grad[i] = df(arg)/d(arg[i])
'    * ndimensional_hess         calculates func = f(arg),
'                                grad[i] = df(arg)/d(arg[i]),
'                                hess[i,j] = d2f(arg)/(d(arg[i])*d(arg[j]))
'    
'    Callbacks for systems of functions:
'    * ndimensional_fvec         calculates vector function f(arg),
'                                stores result to fi
'    * ndimensional_jac          calculates f[i] = fi(arg)
'                                jac[i,j] = df[i](arg)/d(arg[j])
'                                
'    Callbacks for  parameterized  functions,  i.e.  for  functions  which 
'    depend on two vectors: P and Q.  Gradient  and Hessian are calculated 
'    with respect to P only.
'    * ndimensional_pfunc        calculates f(p,q),
'                                stores result to func
'    * ndimensional_pgrad        calculates func = f(p,q),
'                                grad[i] = df(p,q)/d(p[i])
'    * ndimensional_phess        calculates func = f(p,q),
'                                grad[i] = df(p,q)/d(p[i]),
'                                hess[i,j] = d2f(p,q)/(d(p[i])*d(p[j]))
'
'    Callbacks for progress reports:
'    * ndimensional_rep          reports current position of optimization algo    
'    
'    Callbacks for ODE solvers:
'    * ndimensional_ode_rp       calculates dy/dx for given y[] and x
'    
'    Callbacks for integrators:
'    * integrator1_func          calculates f(x) for given x
'                                (additional parameters xminusa and bminusx
'                                contain x-a and b-x)
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
Public Delegate Sub ndimensional_func(arg As Double(), ByRef func As Double, obj As Object)
Public Delegate Sub ndimensional_grad(arg As Double(), ByRef func As Double, grad As Double(), obj As Object)
Public Delegate Sub ndimensional_hess(arg As Double(), ByRef func As Double, grad As Double(), hess As Double(,), obj As Object)

Public Delegate Sub ndimensional_fvec(arg As Double(), fi As Double(), obj As Object)
Public Delegate Sub ndimensional_jac(arg As Double(), fi As Double(), jac As Double(,), obj As Object)

Public Delegate Sub ndimensional_pfunc(p As Double(), q As Double(), ByRef func As Double, obj As Object)
Public Delegate Sub ndimensional_pgrad(p As Double(), q As Double(), ByRef func As Double, grad As Double(), obj As Object)
Public Delegate Sub ndimensional_phess(p As Double(), q As Double(), ByRef func As Double, grad As Double(), hess As Double(,), obj As Object)

Public Delegate Sub ndimensional_rep(arg As Double(), func As Double, obj As Object)

Public Delegate Sub ndimensional_ode_rp(y As Double(), x As Double, dy As Double(), obj As Object)

Public Delegate Sub integrator1_func(x As Double, xminusa As Double, bminusx As Double, ByRef f As Double, obj As Object)

'
' ALGLIB exception
'
Public Class AlglibException
    Inherits System.ApplicationException
    Public Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub
End Class


    Public Class hqrndstate
        Public csobj As alglib.hqrndstate
    End Class


    Public Sub hqrndrandomize(ByRef state As hqrndstate)
        Try
            state = New hqrndstate()
            alglib.hqrndrandomize(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hqrndseed(ByVal s1 As Integer, ByVal s2 As Integer, ByRef state As hqrndstate)
        Try
            state = New hqrndstate()
            alglib.hqrndseed(s1, s2, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function hqrnduniformr(ByVal state As hqrndstate) As Double
        Try
            hqrnduniformr = alglib.hqrnduniformr(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hqrnduniformi(ByVal state As hqrndstate, ByVal n As Integer) As Integer
        Try
            hqrnduniformi = alglib.hqrnduniformi(state.csobj, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hqrndnormal(ByVal state As hqrndstate) As Double
        Try
            hqrndnormal = alglib.hqrndnormal(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub hqrndunit2(ByVal state As hqrndstate, ByRef x As Double, ByRef y As Double)
        Try
            alglib.hqrndunit2(state.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hqrndnormal2(ByVal state As hqrndstate, ByRef x1 As Double, ByRef x2 As Double)
        Try
            alglib.hqrndnormal2(state.csobj, x1, x2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function hqrndexponential(ByVal state As hqrndstate, ByVal lambdav As Double) As Double
        Try
            hqrndexponential = alglib.hqrndexponential(state.csobj, lambdav)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class kdtree
        Public csobj As alglib.kdtree
    End Class


    Public Sub kdtreebuild(ByVal xy(,) As Double, ByVal n As Integer, ByVal nx As Integer, ByVal ny As Integer, ByVal normtype As Integer, ByRef kdt As kdtree)
        Try
            kdt = New kdtree()
            alglib.kdtreebuild(xy, n, nx, ny, normtype, kdt.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreebuild(ByVal xy(,) As Double, ByVal nx As Integer, ByVal ny As Integer, ByVal normtype As Integer, ByRef kdt As kdtree)
        Try
            kdt = New kdtree()
            alglib.kdtreebuild(xy, nx, ny, normtype, kdt.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreebuildtagged(ByVal xy(,) As Double, ByVal tags() As Integer, ByVal n As Integer, ByVal nx As Integer, ByVal ny As Integer, ByVal normtype As Integer, ByRef kdt As kdtree)
        Try
            kdt = New kdtree()
            alglib.kdtreebuildtagged(xy, tags, n, nx, ny, normtype, kdt.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreebuildtagged(ByVal xy(,) As Double, ByVal tags() As Integer, ByVal nx As Integer, ByVal ny As Integer, ByVal normtype As Integer, ByRef kdt As kdtree)
        Try
            kdt = New kdtree()
            alglib.kdtreebuildtagged(xy, tags, nx, ny, normtype, kdt.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function kdtreequeryknn(ByVal kdt As kdtree, ByVal x() As Double, ByVal k As Integer, ByVal selfmatch As Boolean) As Integer
        Try
            kdtreequeryknn = alglib.kdtreequeryknn(kdt.csobj, x, k, selfmatch)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function kdtreequeryknn(ByVal kdt As kdtree, ByVal x() As Double, ByVal k As Integer) As Integer
        Try
            kdtreequeryknn = alglib.kdtreequeryknn(kdt.csobj, x, k)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function kdtreequeryrnn(ByVal kdt As kdtree, ByVal x() As Double, ByVal r As Double, ByVal selfmatch As Boolean) As Integer
        Try
            kdtreequeryrnn = alglib.kdtreequeryrnn(kdt.csobj, x, r, selfmatch)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function kdtreequeryrnn(ByVal kdt As kdtree, ByVal x() As Double, ByVal r As Double) As Integer
        Try
            kdtreequeryrnn = alglib.kdtreequeryrnn(kdt.csobj, x, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function kdtreequeryaknn(ByVal kdt As kdtree, ByVal x() As Double, ByVal k As Integer, ByVal selfmatch As Boolean, ByVal eps As Double) As Integer
        Try
            kdtreequeryaknn = alglib.kdtreequeryaknn(kdt.csobj, x, k, selfmatch, eps)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function kdtreequeryaknn(ByVal kdt As kdtree, ByVal x() As Double, ByVal k As Integer, ByVal eps As Double) As Integer
        Try
            kdtreequeryaknn = alglib.kdtreequeryaknn(kdt.csobj, x, k, eps)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub kdtreequeryresultsx(ByVal kdt As kdtree, ByRef x(,) As Double)
        Try
            alglib.kdtreequeryresultsx(kdt.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultsxy(ByVal kdt As kdtree, ByRef xy(,) As Double)
        Try
            alglib.kdtreequeryresultsxy(kdt.csobj, xy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultstags(ByVal kdt As kdtree, ByRef tags() As Integer)
        Try
            alglib.kdtreequeryresultstags(kdt.csobj, tags)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultsdistances(ByVal kdt As kdtree, ByRef r() As Double)
        Try
            alglib.kdtreequeryresultsdistances(kdt.csobj, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultsxi(ByVal kdt As kdtree, ByRef x(,) As Double)
        Try
            alglib.kdtreequeryresultsxi(kdt.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultsxyi(ByVal kdt As kdtree, ByRef xy(,) As Double)
        Try
            alglib.kdtreequeryresultsxyi(kdt.csobj, xy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultstagsi(ByVal kdt As kdtree, ByRef tags() As Integer)
        Try
            alglib.kdtreequeryresultstagsi(kdt.csobj, tags)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub kdtreequeryresultsdistancesi(ByVal kdt As kdtree, ByRef r() As Double)
        Try
            alglib.kdtreequeryresultsdistancesi(kdt.csobj, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub cmatrixtranspose(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByRef b(,) As alglib.complex, ByVal ib As Integer, ByVal jb As Integer)
        Try
            alglib.cmatrixtranspose(m, n, a, ia, ja, b, ib, jb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixtranspose(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByRef b(,) As Double, ByVal ib As Integer, ByVal jb As Integer)
        Try
            alglib.rmatrixtranspose(m, n, a, ia, ja, b, ib, jb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixcopy(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByRef b(,) As alglib.complex, ByVal ib As Integer, ByVal jb As Integer)
        Try
            alglib.cmatrixcopy(m, n, a, ia, ja, b, ib, jb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixcopy(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByRef b(,) As Double, ByVal ib As Integer, ByVal jb As Integer)
        Try
            alglib.rmatrixcopy(m, n, a, ia, ja, b, ib, jb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrank1(ByVal m As Integer, ByVal n As Integer, ByRef a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByRef u() As alglib.complex, ByVal iu As Integer, ByRef v() As alglib.complex, ByVal iv As Integer)
        Try
            alglib.cmatrixrank1(m, n, a, ia, ja, u, iu, v, iv)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixrank1(ByVal m As Integer, ByVal n As Integer, ByRef a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByRef u() As Double, ByVal iu As Integer, ByRef v() As Double, ByVal iv As Integer)
        Try
            alglib.rmatrixrank1(m, n, a, ia, ja, u, iu, v, iv)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixmv(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByVal opa As Integer, ByVal x() As alglib.complex, ByVal ix As Integer, ByRef y() As alglib.complex, ByVal iy As Integer)
        Try
            alglib.cmatrixmv(m, n, a, ia, ja, opa, x, ix, y, iy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixmv(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByVal opa As Integer, ByVal x() As Double, ByVal ix As Integer, ByRef y() As Double, ByVal iy As Integer)
        Try
            alglib.rmatrixmv(m, n, a, ia, ja, opa, x, ix, y, iy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrighttrsm(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As alglib.complex, ByVal i1 As Integer, ByVal j1 As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByVal optype As Integer, ByRef x(,) As alglib.complex, ByVal i2 As Integer, ByVal j2 As Integer)
        Try
            alglib.cmatrixrighttrsm(m, n, a, i1, j1, isupper, isunit, optype, x, i2, j2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlefttrsm(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As alglib.complex, ByVal i1 As Integer, ByVal j1 As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByVal optype As Integer, ByRef x(,) As alglib.complex, ByVal i2 As Integer, ByVal j2 As Integer)
        Try
            alglib.cmatrixlefttrsm(m, n, a, i1, j1, isupper, isunit, optype, x, i2, j2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixrighttrsm(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As Double, ByVal i1 As Integer, ByVal j1 As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByVal optype As Integer, ByRef x(,) As Double, ByVal i2 As Integer, ByVal j2 As Integer)
        Try
            alglib.rmatrixrighttrsm(m, n, a, i1, j1, isupper, isunit, optype, x, i2, j2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlefttrsm(ByVal m As Integer, ByVal n As Integer, ByVal a(,) As Double, ByVal i1 As Integer, ByVal j1 As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByVal optype As Integer, ByRef x(,) As Double, ByVal i2 As Integer, ByVal j2 As Integer)
        Try
            alglib.rmatrixlefttrsm(m, n, a, i1, j1, isupper, isunit, optype, x, i2, j2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixsyrk(ByVal n As Integer, ByVal k As Integer, ByVal alpha As Double, ByVal a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByVal optypea As Integer, ByVal beta As Double, ByRef c(,) As alglib.complex, ByVal ic As Integer, ByVal jc As Integer, ByVal isupper As Boolean)
        Try
            alglib.cmatrixsyrk(n, k, alpha, a, ia, ja, optypea, beta, c, ic, jc, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixsyrk(ByVal n As Integer, ByVal k As Integer, ByVal alpha As Double, ByVal a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByVal optypea As Integer, ByVal beta As Double, ByRef c(,) As Double, ByVal ic As Integer, ByVal jc As Integer, ByVal isupper As Boolean)
        Try
            alglib.rmatrixsyrk(n, k, alpha, a, ia, ja, optypea, beta, c, ic, jc, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixgemm(ByVal m As Integer, ByVal n As Integer, ByVal k As Integer, ByVal alpha As alglib.complex, ByVal a(,) As alglib.complex, ByVal ia As Integer, ByVal ja As Integer, ByVal optypea As Integer, ByVal b(,) As alglib.complex, ByVal ib As Integer, ByVal jb As Integer, ByVal optypeb As Integer, ByVal beta As alglib.complex, ByRef c(,) As alglib.complex, ByVal ic As Integer, ByVal jc As Integer)
        Try
            alglib.cmatrixgemm(m, n, k, alpha, a, ia, ja, optypea, b, ib, jb, optypeb, beta, c, ic, jc)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixgemm(ByVal m As Integer, ByVal n As Integer, ByVal k As Integer, ByVal alpha As Double, ByVal a(,) As Double, ByVal ia As Integer, ByVal ja As Integer, ByVal optypea As Integer, ByVal b(,) As Double, ByVal ib As Integer, ByVal jb As Integer, ByVal optypeb As Integer, ByVal beta As Double, ByRef c(,) As Double, ByVal ic As Integer, ByVal jc As Integer)
        Try
            alglib.rmatrixgemm(m, n, k, alpha, a, ia, ja, optypea, b, ib, jb, optypeb, beta, c, ic, jc)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub samplemoments(ByVal x() As Double, ByVal n As Integer, ByRef mean As Double, ByRef variance As Double, ByRef skewness As Double, ByRef kurtosis As Double)
        Try
            alglib.samplemoments(x, n, mean, variance, skewness, kurtosis)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub samplemoments(ByVal x() As Double, ByRef mean As Double, ByRef variance As Double, ByRef skewness As Double, ByRef kurtosis As Double)
        Try
            alglib.samplemoments(x, mean, variance, skewness, kurtosis)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub sampleadev(ByVal x() As Double, ByVal n As Integer, ByRef adev As Double)
        Try
            alglib.sampleadev(x, n, adev)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub sampleadev(ByVal x() As Double, ByRef adev As Double)
        Try
            alglib.sampleadev(x, adev)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub samplemedian(ByVal x() As Double, ByVal n As Integer, ByRef median As Double)
        Try
            alglib.samplemedian(x, n, median)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub samplemedian(ByVal x() As Double, ByRef median As Double)
        Try
            alglib.samplemedian(x, median)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub samplepercentile(ByVal x() As Double, ByVal n As Integer, ByVal p As Double, ByRef v As Double)
        Try
            alglib.samplepercentile(x, n, p, v)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub samplepercentile(ByVal x() As Double, ByVal p As Double, ByRef v As Double)
        Try
            alglib.samplepercentile(x, p, v)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function cov2(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer) As Double
        Try
            cov2 = alglib.cov2(x, y, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cov2(ByVal x() As Double, ByVal y() As Double) As Double
        Try
            cov2 = alglib.cov2(x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function pearsoncorr2(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer) As Double
        Try
            pearsoncorr2 = alglib.pearsoncorr2(x, y, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function pearsoncorr2(ByVal x() As Double, ByVal y() As Double) As Double
        Try
            pearsoncorr2 = alglib.pearsoncorr2(x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spearmancorr2(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer) As Double
        Try
            spearmancorr2 = alglib.spearmancorr2(x, y, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spearmancorr2(ByVal x() As Double, ByVal y() As Double) As Double
        Try
            spearmancorr2 = alglib.spearmancorr2(x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub covm(ByVal x(,) As Double, ByVal n As Integer, ByVal m As Integer, ByRef c(,) As Double)
        Try
            alglib.covm(x, n, m, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub covm(ByVal x(,) As Double, ByRef c(,) As Double)
        Try
            alglib.covm(x, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pearsoncorrm(ByVal x(,) As Double, ByVal n As Integer, ByVal m As Integer, ByRef c(,) As Double)
        Try
            alglib.pearsoncorrm(x, n, m, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pearsoncorrm(ByVal x(,) As Double, ByRef c(,) As Double)
        Try
            alglib.pearsoncorrm(x, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spearmancorrm(ByVal x(,) As Double, ByVal n As Integer, ByVal m As Integer, ByRef c(,) As Double)
        Try
            alglib.spearmancorrm(x, n, m, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spearmancorrm(ByVal x(,) As Double, ByRef c(,) As Double)
        Try
            alglib.spearmancorrm(x, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub covm2(ByVal x(,) As Double, ByVal y(,) As Double, ByVal n As Integer, ByVal m1 As Integer, ByVal m2 As Integer, ByRef c(,) As Double)
        Try
            alglib.covm2(x, y, n, m1, m2, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub covm2(ByVal x(,) As Double, ByVal y(,) As Double, ByRef c(,) As Double)
        Try
            alglib.covm2(x, y, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pearsoncorrm2(ByVal x(,) As Double, ByVal y(,) As Double, ByVal n As Integer, ByVal m1 As Integer, ByVal m2 As Integer, ByRef c(,) As Double)
        Try
            alglib.pearsoncorrm2(x, y, n, m1, m2, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pearsoncorrm2(ByVal x(,) As Double, ByVal y(,) As Double, ByRef c(,) As Double)
        Try
            alglib.pearsoncorrm2(x, y, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spearmancorrm2(ByVal x(,) As Double, ByVal y(,) As Double, ByVal n As Integer, ByVal m1 As Integer, ByVal m2 As Integer, ByRef c(,) As Double)
        Try
            alglib.spearmancorrm2(x, y, n, m1, m2, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spearmancorrm2(ByVal x(,) As Double, ByVal y(,) As Double, ByRef c(,) As Double)
        Try
            alglib.spearmancorrm2(x, y, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function pearsoncorrelation(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer) As Double
        Try
            pearsoncorrelation = alglib.pearsoncorrelation(x, y, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spearmanrankcorrelation(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer) As Double
        Try
            spearmanrankcorrelation = alglib.spearmanrankcorrelation(x, y, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub dsoptimalsplit2(ByVal a() As Double, ByVal c() As Integer, ByVal n As Integer, ByRef info As Integer, ByRef threshold As Double, ByRef pal As Double, ByRef pbl As Double, ByRef par As Double, ByRef pbr As Double, ByRef cve As Double)
        Try
            alglib.dsoptimalsplit2(a, c, n, info, threshold, pal, pbl, par, pbr, cve)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub dsoptimalsplit2fast(ByRef a() As Double, ByRef c() As Integer, ByRef tiesbuf() As Integer, ByRef cntbuf() As Integer, ByRef bufr() As Double, ByRef bufi() As Integer, ByVal n As Integer, ByVal nc As Integer, ByVal alpha As Double, ByRef info As Integer, ByRef threshold As Double, ByRef rms As Double, ByRef cvrms As Double)
        Try
            alglib.dsoptimalsplit2fast(a, c, tiesbuf, cntbuf, bufr, bufi, n, nc, alpha, info, threshold, rms, cvrms)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class decisionforest
        Public csobj As alglib.decisionforest
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class dfreport
        Public Property relclserror() As Double
        Get
            Return csobj.relclserror
        End Get
        Set(ByVal Value As Double)
            csobj.relclserror = Value
        End Set
        End Property
        Public Property avgce() As Double
        Get
            Return csobj.avgce
        End Get
        Set(ByVal Value As Double)
            csobj.avgce = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property oobrelclserror() As Double
        Get
            Return csobj.oobrelclserror
        End Get
        Set(ByVal Value As Double)
            csobj.oobrelclserror = Value
        End Set
        End Property
        Public Property oobavgce() As Double
        Get
            Return csobj.oobavgce
        End Get
        Set(ByVal Value As Double)
            csobj.oobavgce = Value
        End Set
        End Property
        Public Property oobrmserror() As Double
        Get
            Return csobj.oobrmserror
        End Get
        Set(ByVal Value As Double)
            csobj.oobrmserror = Value
        End Set
        End Property
        Public Property oobavgerror() As Double
        Get
            Return csobj.oobavgerror
        End Get
        Set(ByVal Value As Double)
            csobj.oobavgerror = Value
        End Set
        End Property
        Public Property oobavgrelerror() As Double
        Get
            Return csobj.oobavgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.oobavgrelerror = Value
        End Set
        End Property
        Public csobj As alglib.dfreport
    End Class


    Public Sub dfbuildrandomdecisionforest(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByVal nclasses As Integer, ByVal ntrees As Integer, ByVal r As Double, ByRef info As Integer, ByRef df As decisionforest, ByRef rep As dfreport)
        Try
            df = New decisionforest()
            rep = New dfreport()
            alglib.dfbuildrandomdecisionforest(xy, npoints, nvars, nclasses, ntrees, r, info, df.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub dfprocess(ByVal df As decisionforest, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.dfprocess(df.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub dfprocessi(ByVal df As decisionforest, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.dfprocessi(df.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function dfrelclserror(ByVal df As decisionforest, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            dfrelclserror = alglib.dfrelclserror(df.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function dfavgce(ByVal df As decisionforest, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            dfavgce = alglib.dfavgce(df.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function dfrmserror(ByVal df As decisionforest, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            dfrmserror = alglib.dfrmserror(df.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function dfavgerror(ByVal df As decisionforest, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            dfavgerror = alglib.dfavgerror(df.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function dfavgrelerror(ByVal df As decisionforest, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            dfavgrelerror = alglib.dfavgrelerror(df.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub kmeansgenerate(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByVal k As Integer, ByVal restarts As Integer, ByRef info As Integer, ByRef c(,) As Double, ByRef xyc() As Integer)
        Try
            alglib.kmeansgenerate(xy, npoints, nvars, k, restarts, info, c, xyc)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub rmatrixqr(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef tau() As Double)
        Try
            alglib.rmatrixqr(a, m, n, tau)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlq(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef tau() As Double)
        Try
            alglib.rmatrixlq(a, m, n, tau)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixqr(ByRef a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByRef tau() As alglib.complex)
        Try
            alglib.cmatrixqr(a, m, n, tau)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlq(ByRef a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByRef tau() As alglib.complex)
        Try
            alglib.cmatrixlq(a, m, n, tau)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixqrunpackq(ByVal a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal tau() As Double, ByVal qcolumns As Integer, ByRef q(,) As Double)
        Try
            alglib.rmatrixqrunpackq(a, m, n, tau, qcolumns, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixqrunpackr(ByVal a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef r(,) As Double)
        Try
            alglib.rmatrixqrunpackr(a, m, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlqunpackq(ByVal a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal tau() As Double, ByVal qrows As Integer, ByRef q(,) As Double)
        Try
            alglib.rmatrixlqunpackq(a, m, n, tau, qrows, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlqunpackl(ByVal a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef l(,) As Double)
        Try
            alglib.rmatrixlqunpackl(a, m, n, l)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixqrunpackq(ByVal a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByVal tau() As alglib.complex, ByVal qcolumns As Integer, ByRef q(,) As alglib.complex)
        Try
            alglib.cmatrixqrunpackq(a, m, n, tau, qcolumns, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixqrunpackr(ByVal a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByRef r(,) As alglib.complex)
        Try
            alglib.cmatrixqrunpackr(a, m, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlqunpackq(ByVal a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByVal tau() As alglib.complex, ByVal qrows As Integer, ByRef q(,) As alglib.complex)
        Try
            alglib.cmatrixlqunpackq(a, m, n, tau, qrows, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlqunpackl(ByVal a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByRef l(,) As alglib.complex)
        Try
            alglib.cmatrixlqunpackl(a, m, n, l)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbd(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef tauq() As Double, ByRef taup() As Double)
        Try
            alglib.rmatrixbd(a, m, n, tauq, taup)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbdunpackq(ByVal qp(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal tauq() As Double, ByVal qcolumns As Integer, ByRef q(,) As Double)
        Try
            alglib.rmatrixbdunpackq(qp, m, n, tauq, qcolumns, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbdmultiplybyq(ByVal qp(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal tauq() As Double, ByRef z(,) As Double, ByVal zrows As Integer, ByVal zcolumns As Integer, ByVal fromtheright As Boolean, ByVal dotranspose As Boolean)
        Try
            alglib.rmatrixbdmultiplybyq(qp, m, n, tauq, z, zrows, zcolumns, fromtheright, dotranspose)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbdunpackpt(ByVal qp(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal taup() As Double, ByVal ptrows As Integer, ByRef pt(,) As Double)
        Try
            alglib.rmatrixbdunpackpt(qp, m, n, taup, ptrows, pt)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbdmultiplybyp(ByVal qp(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal taup() As Double, ByRef z(,) As Double, ByVal zrows As Integer, ByVal zcolumns As Integer, ByVal fromtheright As Boolean, ByVal dotranspose As Boolean)
        Try
            alglib.rmatrixbdmultiplybyp(qp, m, n, taup, z, zrows, zcolumns, fromtheright, dotranspose)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixbdunpackdiagonals(ByVal b(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef isupper As Boolean, ByRef d() As Double, ByRef e() As Double)
        Try
            alglib.rmatrixbdunpackdiagonals(b, m, n, isupper, d, e)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixhessenberg(ByRef a(,) As Double, ByVal n As Integer, ByRef tau() As Double)
        Try
            alglib.rmatrixhessenberg(a, n, tau)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixhessenbergunpackq(ByVal a(,) As Double, ByVal n As Integer, ByVal tau() As Double, ByRef q(,) As Double)
        Try
            alglib.rmatrixhessenbergunpackq(a, n, tau, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixhessenbergunpackh(ByVal a(,) As Double, ByVal n As Integer, ByRef h(,) As Double)
        Try
            alglib.rmatrixhessenbergunpackh(a, n, h)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub smatrixtd(ByRef a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByRef tau() As Double, ByRef d() As Double, ByRef e() As Double)
        Try
            alglib.smatrixtd(a, n, isupper, tau, d, e)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub smatrixtdunpackq(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal tau() As Double, ByRef q(,) As Double)
        Try
            alglib.smatrixtdunpackq(a, n, isupper, tau, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hmatrixtd(ByRef a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByRef tau() As alglib.complex, ByRef d() As Double, ByRef e() As Double)
        Try
            alglib.hmatrixtd(a, n, isupper, tau, d, e)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hmatrixtdunpackq(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal tau() As alglib.complex, ByRef q(,) As alglib.complex)
        Try
            alglib.hmatrixtdunpackq(a, n, isupper, tau, q)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function smatrixevd(ByVal a(,) As Double, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByRef d() As Double, ByRef z(,) As Double) As Boolean
        Try
            smatrixevd = alglib.smatrixevd(a, n, zneeded, isupper, d, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixevdr(ByVal a(,) As Double, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByVal b1 As Double, ByVal b2 As Double, ByRef m As Integer, ByRef w() As Double, ByRef z(,) As Double) As Boolean
        Try
            smatrixevdr = alglib.smatrixevdr(a, n, zneeded, isupper, b1, b2, m, w, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixevdi(ByVal a(,) As Double, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByVal i1 As Integer, ByVal i2 As Integer, ByRef w() As Double, ByRef z(,) As Double) As Boolean
        Try
            smatrixevdi = alglib.smatrixevdi(a, n, zneeded, isupper, i1, i2, w, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hmatrixevd(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByRef d() As Double, ByRef z(,) As alglib.complex) As Boolean
        Try
            hmatrixevd = alglib.hmatrixevd(a, n, zneeded, isupper, d, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hmatrixevdr(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByVal b1 As Double, ByVal b2 As Double, ByRef m As Integer, ByRef w() As Double, ByRef z(,) As alglib.complex) As Boolean
        Try
            hmatrixevdr = alglib.hmatrixevdr(a, n, zneeded, isupper, b1, b2, m, w, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hmatrixevdi(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal zneeded As Integer, ByVal isupper As Boolean, ByVal i1 As Integer, ByVal i2 As Integer, ByRef w() As Double, ByRef z(,) As alglib.complex) As Boolean
        Try
            hmatrixevdi = alglib.hmatrixevdi(a, n, zneeded, isupper, i1, i2, w, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixtdevd(ByRef d() As Double, ByVal e() As Double, ByVal n As Integer, ByVal zneeded As Integer, ByRef z(,) As Double) As Boolean
        Try
            smatrixtdevd = alglib.smatrixtdevd(d, e, n, zneeded, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixtdevdr(ByRef d() As Double, ByVal e() As Double, ByVal n As Integer, ByVal zneeded As Integer, ByVal a As Double, ByVal b As Double, ByRef m As Integer, ByRef z(,) As Double) As Boolean
        Try
            smatrixtdevdr = alglib.smatrixtdevdr(d, e, n, zneeded, a, b, m, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixtdevdi(ByRef d() As Double, ByVal e() As Double, ByVal n As Integer, ByVal zneeded As Integer, ByVal i1 As Integer, ByVal i2 As Integer, ByRef z(,) As Double) As Boolean
        Try
            smatrixtdevdi = alglib.smatrixtdevdi(d, e, n, zneeded, i1, i2, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixevd(ByVal a(,) As Double, ByVal n As Integer, ByVal vneeded As Integer, ByRef wr() As Double, ByRef wi() As Double, ByRef vl(,) As Double, ByRef vr(,) As Double) As Boolean
        Try
            rmatrixevd = alglib.rmatrixevd(a, n, vneeded, wr, wi, vl, vr)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub rmatrixrndorthogonal(ByVal n As Integer, ByRef a(,) As Double)
        Try
            alglib.rmatrixrndorthogonal(n, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As Double)
        Try
            alglib.rmatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrndorthogonal(ByVal n As Integer, ByRef a(,) As alglib.complex)
        Try
            alglib.cmatrixrndorthogonal(n, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As alglib.complex)
        Try
            alglib.cmatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub smatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As Double)
        Try
            alglib.smatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As Double)
        Try
            alglib.spdmatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hmatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As alglib.complex)
        Try
            alglib.hmatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixrndcond(ByVal n As Integer, ByVal c As Double, ByRef a(,) As alglib.complex)
        Try
            alglib.hpdmatrixrndcond(n, c, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixrndorthogonalfromtheright(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer)
        Try
            alglib.rmatrixrndorthogonalfromtheright(a, m, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixrndorthogonalfromtheleft(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer)
        Try
            alglib.rmatrixrndorthogonalfromtheleft(a, m, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrndorthogonalfromtheright(ByRef a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer)
        Try
            alglib.cmatrixrndorthogonalfromtheright(a, m, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixrndorthogonalfromtheleft(ByRef a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer)
        Try
            alglib.cmatrixrndorthogonalfromtheleft(a, m, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub smatrixrndmultiply(ByRef a(,) As Double, ByVal n As Integer)
        Try
            alglib.smatrixrndmultiply(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hmatrixrndmultiply(ByRef a(,) As alglib.complex, ByVal n As Integer)
        Try
            alglib.hmatrixrndmultiply(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub rmatrixlu(ByRef a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef pivots() As Integer)
        Try
            alglib.rmatrixlu(a, m, n, pivots)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlu(ByRef a(,) As alglib.complex, ByVal m As Integer, ByVal n As Integer, ByRef pivots() As Integer)
        Try
            alglib.cmatrixlu(a, m, n, pivots)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function hpdmatrixcholesky(ByRef a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean) As Boolean
        Try
            hpdmatrixcholesky = alglib.hpdmatrixcholesky(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixcholesky(ByRef a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean) As Boolean
        Try
            spdmatrixcholesky = alglib.spdmatrixcholesky(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function rmatrixrcond1(ByVal a(,) As Double, ByVal n As Integer) As Double
        Try
            rmatrixrcond1 = alglib.rmatrixrcond1(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixrcondinf(ByVal a(,) As Double, ByVal n As Integer) As Double
        Try
            rmatrixrcondinf = alglib.rmatrixrcondinf(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixrcond(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean) As Double
        Try
            spdmatrixrcond = alglib.spdmatrixrcond(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixtrrcond1(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean) As Double
        Try
            rmatrixtrrcond1 = alglib.rmatrixtrrcond1(a, n, isupper, isunit)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixtrrcondinf(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean) As Double
        Try
            rmatrixtrrcondinf = alglib.rmatrixtrrcondinf(a, n, isupper, isunit)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hpdmatrixrcond(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean) As Double
        Try
            hpdmatrixrcond = alglib.hpdmatrixrcond(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixrcond1(ByVal a(,) As alglib.complex, ByVal n As Integer) As Double
        Try
            cmatrixrcond1 = alglib.cmatrixrcond1(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixrcondinf(ByVal a(,) As alglib.complex, ByVal n As Integer) As Double
        Try
            cmatrixrcondinf = alglib.cmatrixrcondinf(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixlurcond1(ByVal lua(,) As Double, ByVal n As Integer) As Double
        Try
            rmatrixlurcond1 = alglib.rmatrixlurcond1(lua, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixlurcondinf(ByVal lua(,) As Double, ByVal n As Integer) As Double
        Try
            rmatrixlurcondinf = alglib.rmatrixlurcondinf(lua, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixcholeskyrcond(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean) As Double
        Try
            spdmatrixcholeskyrcond = alglib.spdmatrixcholeskyrcond(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hpdmatrixcholeskyrcond(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean) As Double
        Try
            hpdmatrixcholeskyrcond = alglib.hpdmatrixcholeskyrcond(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixlurcond1(ByVal lua(,) As alglib.complex, ByVal n As Integer) As Double
        Try
            cmatrixlurcond1 = alglib.cmatrixlurcond1(lua, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixlurcondinf(ByVal lua(,) As alglib.complex, ByVal n As Integer) As Double
        Try
            cmatrixlurcondinf = alglib.cmatrixlurcondinf(lua, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixtrrcond1(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean) As Double
        Try
            cmatrixtrrcond1 = alglib.cmatrixtrrcond1(a, n, isupper, isunit)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixtrrcondinf(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean) As Double
        Try
            cmatrixtrrcondinf = alglib.cmatrixtrrcondinf(a, n, isupper, isunit)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Matrix inverse report:
    '* R1    reciprocal of condition number in 1-norm
    '* RInf  reciprocal of condition number in inf-norm
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class matinvreport
        Public Property r1() As Double
        Get
            Return csobj.r1
        End Get
        Set(ByVal Value As Double)
            csobj.r1 = Value
        End Set
        End Property
        Public Property rinf() As Double
        Get
            Return csobj.rinf
        End Get
        Set(ByVal Value As Double)
            csobj.rinf = Value
        End Set
        End Property
        Public csobj As alglib.matinvreport
    End Class


    Public Sub rmatrixluinverse(ByRef a(,) As Double, ByVal pivots() As Integer, ByVal n As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixluinverse(a, pivots, n, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixluinverse(ByRef a(,) As Double, ByVal pivots() As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixluinverse(a, pivots, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixinverse(ByRef a(,) As Double, ByVal n As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixinverse(a, n, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixinverse(ByRef a(,) As Double, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixluinverse(ByRef a(,) As alglib.complex, ByVal pivots() As Integer, ByVal n As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixluinverse(a, pivots, n, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixluinverse(ByRef a(,) As alglib.complex, ByVal pivots() As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixluinverse(a, pivots, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixinverse(ByRef a(,) As alglib.complex, ByVal n As Integer, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixinverse(a, n, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixinverse(ByRef a(,) As alglib.complex, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixcholeskyinverse(ByRef a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.spdmatrixcholeskyinverse(a, n, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixcholeskyinverse(ByRef a(,) As Double, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.spdmatrixcholeskyinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixinverse(ByRef a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.spdmatrixinverse(a, n, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixinverse(ByRef a(,) As Double, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.spdmatrixinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixcholeskyinverse(ByRef a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.hpdmatrixcholeskyinverse(a, n, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixcholeskyinverse(ByRef a(,) As alglib.complex, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.hpdmatrixcholeskyinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixinverse(ByRef a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.hpdmatrixinverse(a, n, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixinverse(ByRef a(,) As alglib.complex, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.hpdmatrixinverse(a, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixtrinverse(ByRef a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixtrinverse(a, n, isupper, isunit, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixtrinverse(ByRef a(,) As Double, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.rmatrixtrinverse(a, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixtrinverse(ByRef a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal isunit As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixtrinverse(a, n, isupper, isunit, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixtrinverse(ByRef a(,) As alglib.complex, ByVal isupper As Boolean, ByRef info As Integer, ByRef rep As matinvreport)
        Try
            rep = New matinvreport()
            alglib.cmatrixtrinverse(a, isupper, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub fisherlda(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByVal nclasses As Integer, ByRef info As Integer, ByRef w() As Double)
        Try
            alglib.fisherlda(xy, npoints, nvars, nclasses, info, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fisherldan(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByVal nclasses As Integer, ByRef info As Integer, ByRef w(,) As Double)
        Try
            alglib.fisherldan(xy, npoints, nvars, nclasses, info, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function gammafunction(ByVal x As Double) As Double
        Try
            gammafunction = alglib.gammafunction(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function lngamma(ByVal x As Double, ByRef sgngam As Double) As Double
        Try
            lngamma = alglib.lngamma(x, sgngam)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function errorfunction(ByVal x As Double) As Double
        Try
            errorfunction = alglib.errorfunction(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function errorfunctionc(ByVal x As Double) As Double
        Try
            errorfunctionc = alglib.errorfunctionc(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function normaldistribution(ByVal x As Double) As Double
        Try
            normaldistribution = alglib.normaldistribution(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function inverf(ByVal e As Double) As Double
        Try
            inverf = alglib.inverf(e)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invnormaldistribution(ByVal y0 As Double) As Double
        Try
            invnormaldistribution = alglib.invnormaldistribution(y0)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function incompletegamma(ByVal a As Double, ByVal x As Double) As Double
        Try
            incompletegamma = alglib.incompletegamma(a, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function incompletegammac(ByVal a As Double, ByVal x As Double) As Double
        Try
            incompletegammac = alglib.incompletegammac(a, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invincompletegammac(ByVal a As Double, ByVal y0 As Double) As Double
        Try
            invincompletegammac = alglib.invincompletegammac(a, y0)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function rmatrixbdsvd(ByRef d() As Double, ByVal e() As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal isfractionalaccuracyrequired As Boolean, ByRef u(,) As Double, ByVal nru As Integer, ByRef c(,) As Double, ByVal ncc As Integer, ByRef vt(,) As Double, ByVal ncvt As Integer) As Boolean
        Try
            rmatrixbdsvd = alglib.rmatrixbdsvd(d, e, n, isupper, isfractionalaccuracyrequired, u, nru, c, ncc, vt, ncvt)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function rmatrixsvd(ByVal a(,) As Double, ByVal m As Integer, ByVal n As Integer, ByVal uneeded As Integer, ByVal vtneeded As Integer, ByVal additionalmemory As Integer, ByRef w() As Double, ByRef u(,) As Double, ByRef vt(,) As Double) As Boolean
        Try
            rmatrixsvd = alglib.rmatrixsvd(a, m, n, uneeded, vtneeded, additionalmemory, w, u, vt)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class linearmodel
        Public csobj As alglib.linearmodel
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'LRReport structure contains additional information about linear model:
    '* C             -   covariation matrix,  array[0..NVars,0..NVars].
    '                    C[i,j] = Cov(A[i],A[j])
    '* RMSError      -   root mean square error on a training set
    '* AvgError      -   average error on a training set
    '* AvgRelError   -   average relative error on a training set (excluding
    '                    observations with zero function value).
    '* CVRMSError    -   leave-one-out cross-validation estimate of
    '                    generalization error. Calculated using fast algorithm
    '                    with O(NVars*NPoints) complexity.
    '* CVAvgError    -   cross-validation estimate of average error
    '* CVAvgRelError -   cross-validation estimate of average relative error
    '
    'All other fields of the structure are intended for internal use and should
    'not be used outside ALGLIB.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class lrreport
        Public Property c() As Double(,)
        Get
            Return csobj.c
        End Get
        Set(ByVal Value As Double(,))
            csobj.c = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property cvrmserror() As Double
        Get
            Return csobj.cvrmserror
        End Get
        Set(ByVal Value As Double)
            csobj.cvrmserror = Value
        End Set
        End Property
        Public Property cvavgerror() As Double
        Get
            Return csobj.cvavgerror
        End Get
        Set(ByVal Value As Double)
            csobj.cvavgerror = Value
        End Set
        End Property
        Public Property cvavgrelerror() As Double
        Get
            Return csobj.cvavgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.cvavgrelerror = Value
        End Set
        End Property
        Public Property ncvdefects() As Integer
        Get
            Return csobj.ncvdefects
        End Get
        Set(ByVal Value As Integer)
            csobj.ncvdefects = Value
        End Set
        End Property
        Public Property cvdefects() As Integer()
        Get
            Return csobj.cvdefects
        End Get
        Set(ByVal Value As Integer())
            csobj.cvdefects = Value
        End Set
        End Property
        Public csobj As alglib.lrreport
    End Class


    Public Sub lrbuild(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByRef info As Integer, ByRef lm As linearmodel, ByRef ar As lrreport)
        Try
            lm = New linearmodel()
            ar = New lrreport()
            alglib.lrbuild(xy, npoints, nvars, info, lm.csobj, ar.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lrbuilds(ByVal xy(,) As Double, ByVal s() As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByRef info As Integer, ByRef lm As linearmodel, ByRef ar As lrreport)
        Try
            lm = New linearmodel()
            ar = New lrreport()
            alglib.lrbuilds(xy, s, npoints, nvars, info, lm.csobj, ar.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lrbuildzs(ByVal xy(,) As Double, ByVal s() As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByRef info As Integer, ByRef lm As linearmodel, ByRef ar As lrreport)
        Try
            lm = New linearmodel()
            ar = New lrreport()
            alglib.lrbuildzs(xy, s, npoints, nvars, info, lm.csobj, ar.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lrbuildz(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByRef info As Integer, ByRef lm As linearmodel, ByRef ar As lrreport)
        Try
            lm = New linearmodel()
            ar = New lrreport()
            alglib.lrbuildz(xy, npoints, nvars, info, lm.csobj, ar.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lrunpack(ByVal lm As linearmodel, ByRef v() As Double, ByRef nvars As Integer)
        Try
            alglib.lrunpack(lm.csobj, v, nvars)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lrpack(ByVal v() As Double, ByVal nvars As Integer, ByRef lm As linearmodel)
        Try
            lm = New linearmodel()
            alglib.lrpack(v, nvars, lm.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function lrprocess(ByVal lm As linearmodel, ByVal x() As Double) As Double
        Try
            lrprocess = alglib.lrprocess(lm.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function lrrmserror(ByVal lm As linearmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            lrrmserror = alglib.lrrmserror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function lravgerror(ByVal lm As linearmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            lravgerror = alglib.lravgerror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function lravgrelerror(ByVal lm As linearmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            lravgrelerror = alglib.lravgrelerror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class multilayerperceptron
        Public csobj As alglib.multilayerperceptron
    End Class


    Public Sub mlpcreate0(ByVal nin As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreate0(nin, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreate1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreate1(nin, nhid, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreate2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreate2(nin, nhid1, nhid2, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreateb0(ByVal nin As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreateb0(nin, nout, b, d, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreateb1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreateb1(nin, nhid, nout, b, d, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreateb2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreateb2(nin, nhid1, nhid2, nout, b, d, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreater0(ByVal nin As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreater0(nin, nout, a, b, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreater1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreater1(nin, nhid, nout, a, b, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreater2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreater2(nin, nhid1, nhid2, nout, a, b, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreatec0(ByVal nin As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreatec0(nin, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreatec1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreatec1(nin, nhid, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpcreatec2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByRef network As multilayerperceptron)
        Try
            network = New multilayerperceptron()
            alglib.mlpcreatec2(nin, nhid1, nhid2, nout, network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlprandomize(ByVal network As multilayerperceptron)
        Try
            alglib.mlprandomize(network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlprandomizefull(ByVal network As multilayerperceptron)
        Try
            alglib.mlprandomizefull(network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpproperties(ByVal network As multilayerperceptron, ByRef nin As Integer, ByRef nout As Integer, ByRef wcount As Integer)
        Try
            alglib.mlpproperties(network.csobj, nin, nout, wcount)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mlpissoftmax(ByVal network As multilayerperceptron) As Boolean
        Try
            mlpissoftmax = alglib.mlpissoftmax(network.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub mlpprocess(ByVal network As multilayerperceptron, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mlpprocess(network.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpprocessi(ByVal network As multilayerperceptron, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mlpprocessi(network.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mlperror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer) As Double
        Try
            mlperror = alglib.mlperror(network.csobj, xy, ssize)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlperrorn(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer) As Double
        Try
            mlperrorn = alglib.mlperrorn(network.csobj, xy, ssize)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpclserror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer) As Integer
        Try
            mlpclserror = alglib.mlpclserror(network.csobj, xy, ssize)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlprelclserror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlprelclserror = alglib.mlprelclserror(network.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpavgce(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpavgce = alglib.mlpavgce(network.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlprmserror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlprmserror = alglib.mlprmserror(network.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpavgerror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpavgerror = alglib.mlpavgerror(network.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpavgrelerror(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpavgrelerror = alglib.mlpavgrelerror(network.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub mlpgrad(ByVal network As multilayerperceptron, ByVal x() As Double, ByVal desiredy() As Double, ByRef e As Double, ByRef grad() As Double)
        Try
            alglib.mlpgrad(network.csobj, x, desiredy, e, grad)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpgradn(ByVal network As multilayerperceptron, ByVal x() As Double, ByVal desiredy() As Double, ByRef e As Double, ByRef grad() As Double)
        Try
            alglib.mlpgradn(network.csobj, x, desiredy, e, grad)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpgradbatch(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer, ByRef e As Double, ByRef grad() As Double)
        Try
            alglib.mlpgradbatch(network.csobj, xy, ssize, e, grad)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpgradnbatch(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer, ByRef e As Double, ByRef grad() As Double)
        Try
            alglib.mlpgradnbatch(network.csobj, xy, ssize, e, grad)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlphessiannbatch(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer, ByRef e As Double, ByRef grad() As Double, ByRef h(,) As Double)
        Try
            alglib.mlphessiannbatch(network.csobj, xy, ssize, e, grad, h)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlphessianbatch(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal ssize As Integer, ByRef e As Double, ByRef grad() As Double, ByRef h(,) As Double)
        Try
            alglib.mlphessianbatch(network.csobj, xy, ssize, e, grad, h)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class densesolverreport
        Public Property r1() As Double
        Get
            Return csobj.r1
        End Get
        Set(ByVal Value As Double)
            csobj.r1 = Value
        End Set
        End Property
        Public Property rinf() As Double
        Get
            Return csobj.rinf
        End Get
        Set(ByVal Value As Double)
            csobj.rinf = Value
        End Set
        End Property
        Public csobj As alglib.densesolverreport
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class densesolverlsreport
        Public Property r2() As Double
        Get
            Return csobj.r2
        End Get
        Set(ByVal Value As Double)
            csobj.r2 = Value
        End Set
        End Property
        Public Property cx() As Double(,)
        Get
            Return csobj.cx
        End Get
        Set(ByVal Value As Double(,))
            csobj.cx = Value
        End Set
        End Property
        Public Property n() As Integer
        Get
            Return csobj.n
        End Get
        Set(ByVal Value As Integer)
            csobj.n = Value
        End Set
        End Property
        Public Property k() As Integer
        Get
            Return csobj.k
        End Get
        Set(ByVal Value As Integer)
            csobj.k = Value
        End Set
        End Property
        Public csobj As alglib.densesolverlsreport
    End Class


    Public Sub rmatrixsolve(ByVal a(,) As Double, ByVal n As Integer, ByVal b() As Double, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixsolve(a, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixsolvem(ByVal a(,) As Double, ByVal n As Integer, ByVal b(,) As Double, ByVal m As Integer, ByVal rfs As Boolean, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixsolvem(a, n, b, m, rfs, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlusolve(ByVal lua(,) As Double, ByVal p() As Integer, ByVal n As Integer, ByVal b() As Double, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixlusolve(lua, p, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixlusolvem(ByVal lua(,) As Double, ByVal p() As Integer, ByVal n As Integer, ByVal b(,) As Double, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixlusolvem(lua, p, n, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixmixedsolve(ByVal a(,) As Double, ByVal lua(,) As Double, ByVal p() As Integer, ByVal n As Integer, ByVal b() As Double, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixmixedsolve(a, lua, p, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixmixedsolvem(ByVal a(,) As Double, ByVal lua(,) As Double, ByVal p() As Integer, ByVal n As Integer, ByVal b(,) As Double, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As Double)
        Try
            rep = New densesolverreport()
            alglib.rmatrixmixedsolvem(a, lua, p, n, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixsolvem(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal b(,) As alglib.complex, ByVal m As Integer, ByVal rfs As Boolean, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixsolvem(a, n, b, m, rfs, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixsolve(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal b() As alglib.complex, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixsolve(a, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlusolvem(ByVal lua(,) As alglib.complex, ByVal p() As Integer, ByVal n As Integer, ByVal b(,) As alglib.complex, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixlusolvem(lua, p, n, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixlusolve(ByVal lua(,) As alglib.complex, ByVal p() As Integer, ByVal n As Integer, ByVal b() As alglib.complex, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixlusolve(lua, p, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixmixedsolvem(ByVal a(,) As alglib.complex, ByVal lua(,) As alglib.complex, ByVal p() As Integer, ByVal n As Integer, ByVal b(,) As alglib.complex, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixmixedsolvem(a, lua, p, n, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub cmatrixmixedsolve(ByVal a(,) As alglib.complex, ByVal lua(,) As alglib.complex, ByVal p() As Integer, ByVal n As Integer, ByVal b() As alglib.complex, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.cmatrixmixedsolve(a, lua, p, n, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixsolvem(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal b(,) As Double, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As Double)
        Try
            rep = New densesolverreport()
            alglib.spdmatrixsolvem(a, n, isupper, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixsolve(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal b() As Double, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As Double)
        Try
            rep = New densesolverreport()
            alglib.spdmatrixsolve(a, n, isupper, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixcholeskysolvem(ByVal cha(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal b(,) As Double, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As Double)
        Try
            rep = New densesolverreport()
            alglib.spdmatrixcholeskysolvem(cha, n, isupper, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spdmatrixcholeskysolve(ByVal cha(,) As Double, ByVal n As Integer, ByVal isupper As Boolean, ByVal b() As Double, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As Double)
        Try
            rep = New densesolverreport()
            alglib.spdmatrixcholeskysolve(cha, n, isupper, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixsolvem(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal b(,) As alglib.complex, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.hpdmatrixsolvem(a, n, isupper, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixsolve(ByVal a(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal b() As alglib.complex, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.hpdmatrixsolve(a, n, isupper, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixcholeskysolvem(ByVal cha(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal b(,) As alglib.complex, ByVal m As Integer, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x(,) As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.hpdmatrixcholeskysolvem(cha, n, isupper, b, m, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hpdmatrixcholeskysolve(ByVal cha(,) As alglib.complex, ByVal n As Integer, ByVal isupper As Boolean, ByVal b() As alglib.complex, ByRef info As Integer, ByRef rep As densesolverreport, ByRef x() As alglib.complex)
        Try
            rep = New densesolverreport()
            alglib.hpdmatrixcholeskysolve(cha, n, isupper, b, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixsolvels(ByVal a(,) As Double, ByVal nrows As Integer, ByVal ncols As Integer, ByVal b() As Double, ByVal threshold As Double, ByRef info As Integer, ByRef rep As densesolverlsreport, ByRef x() As Double)
        Try
            rep = New densesolverlsreport()
            alglib.rmatrixsolvels(a, nrows, ncols, b, threshold, info, rep.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class logitmodel
        Public csobj As alglib.logitmodel
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'MNLReport structure contains information about training process:
    '* NGrad     -   number of gradient calculations
    '* NHess     -   number of Hessian calculations
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class mnlreport
        Public Property ngrad() As Integer
        Get
            Return csobj.ngrad
        End Get
        Set(ByVal Value As Integer)
            csobj.ngrad = Value
        End Set
        End Property
        Public Property nhess() As Integer
        Get
            Return csobj.nhess
        End Get
        Set(ByVal Value As Integer)
            csobj.nhess = Value
        End Set
        End Property
        Public csobj As alglib.mnlreport
    End Class


    Public Sub mnltrainh(ByVal xy(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByVal nclasses As Integer, ByRef info As Integer, ByRef lm As logitmodel, ByRef rep As mnlreport)
        Try
            lm = New logitmodel()
            rep = New mnlreport()
            alglib.mnltrainh(xy, npoints, nvars, nclasses, info, lm.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mnlprocess(ByVal lm As logitmodel, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mnlprocess(lm.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mnlprocessi(ByVal lm As logitmodel, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mnlprocessi(lm.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mnlunpack(ByVal lm As logitmodel, ByRef a(,) As Double, ByRef nvars As Integer, ByRef nclasses As Integer)
        Try
            alglib.mnlunpack(lm.csobj, a, nvars, nclasses)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mnlpack(ByVal a(,) As Double, ByVal nvars As Integer, ByVal nclasses As Integer, ByRef lm As logitmodel)
        Try
            lm = New logitmodel()
            alglib.mnlpack(a, nvars, nclasses, lm.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mnlavgce(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mnlavgce = alglib.mnlavgce(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mnlrelclserror(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mnlrelclserror = alglib.mnlrelclserror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mnlrmserror(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mnlrmserror = alglib.mnlrmserror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mnlavgerror(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mnlavgerror = alglib.mnlavgerror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mnlavgrelerror(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal ssize As Integer) As Double
        Try
            mnlavgrelerror = alglib.mnlavgrelerror(lm.csobj, xy, ssize)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mnlclserror(ByVal lm As logitmodel, ByVal xy(,) As Double, ByVal npoints As Integer) As Integer
        Try
            mnlclserror = alglib.mnlclserror(lm.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Class minlbfgsstate
        Public csobj As alglib.minlbfgsstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class minlbfgsreport
        Public Property iterationscount() As Integer
        Get
            Return csobj.iterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.iterationscount = Value
        End Set
        End Property
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public csobj As alglib.minlbfgsreport
    End Class


    Public Sub minlbfgscreate(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As minlbfgsstate)
        Try
            state = New minlbfgsstate()
            alglib.minlbfgscreate(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgscreate(ByVal m As Integer, ByVal x() As Double, ByRef state As minlbfgsstate)
        Try
            state = New minlbfgsstate()
            alglib.minlbfgscreate(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgssetcond(ByVal state As minlbfgsstate, ByVal epsg As Double, ByVal epsf As Double, ByVal epsx As Double, ByVal maxits As Integer)
        Try
            alglib.minlbfgssetcond(state.csobj, epsg, epsf, epsx, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgssetxrep(ByVal state As minlbfgsstate, ByVal needxrep As Boolean)
        Try
            alglib.minlbfgssetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgssetstpmax(ByVal state As minlbfgsstate, ByVal stpmax As Double)
        Try
            alglib.minlbfgssetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgssetdefaultpreconditioner(ByVal state As minlbfgsstate)
        Try
            alglib.minlbfgssetdefaultpreconditioner(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgssetcholeskypreconditioner(ByVal state As minlbfgsstate, ByVal p(,) As Double, ByVal isupper As Boolean)
        Try
            alglib.minlbfgssetcholeskypreconditioner(state.csobj, p, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function minlbfgsiteration(ByVal state As minlbfgsstate) As Boolean
        Try
            minlbfgsiteration = alglib.minlbfgsiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear optimizer
    ' 
    ' These functions accept following parameters:
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' 
    '   -- ALGLIB --
    '      Copyright 20.03.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub minlbfgsoptimize(ByVal state As minlbfgsstate, ByVal grad As ndimensional_grad, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlbfgs.minlbfgsstate = state.csobj.innerobj
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlbfgsoptimize()' (grad is null)")
        End If
        Try
            While alglib.minlbfgs.minlbfgsiteration(innerobj)
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlbfgsoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub minlbfgsresults(ByVal state As minlbfgsstate, ByRef x() As Double, ByRef rep As minlbfgsreport)
        Try
            rep = New minlbfgsreport()
            alglib.minlbfgsresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgsresultsbuf(ByVal state As minlbfgsstate, ByRef x() As Double, ByRef rep As minlbfgsreport)
        Try
            alglib.minlbfgsresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlbfgsrestartfrom(ByVal state As minlbfgsstate, ByVal x() As Double)
        Try
            alglib.minlbfgsrestartfrom(state.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Training report:
    '    * NGrad     - number of gradient calculations
    '    * NHess     - number of Hessian calculations
    '    * NCholesky - number of Cholesky decompositions
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class mlpreport
        Public Property ngrad() As Integer
        Get
            Return csobj.ngrad
        End Get
        Set(ByVal Value As Integer)
            csobj.ngrad = Value
        End Set
        End Property
        Public Property nhess() As Integer
        Get
            Return csobj.nhess
        End Get
        Set(ByVal Value As Integer)
            csobj.nhess = Value
        End Set
        End Property
        Public Property ncholesky() As Integer
        Get
            Return csobj.ncholesky
        End Get
        Set(ByVal Value As Integer)
            csobj.ncholesky = Value
        End Set
        End Property
        Public csobj As alglib.mlpreport
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Cross-validation estimates of generalization error
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class mlpcvreport
        Public Property relclserror() As Double
        Get
            Return csobj.relclserror
        End Get
        Set(ByVal Value As Double)
            csobj.relclserror = Value
        End Set
        End Property
        Public Property avgce() As Double
        Get
            Return csobj.avgce
        End Get
        Set(ByVal Value As Double)
            csobj.avgce = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public csobj As alglib.mlpcvreport
    End Class


    Public Sub mlptrainlm(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByRef info As Integer, ByRef rep As mlpreport)
        Try
            rep = New mlpreport()
            alglib.mlptrainlm(network.csobj, xy, npoints, decay, restarts, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlptrainlbfgs(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByVal wstep As Double, ByVal maxits As Integer, ByRef info As Integer, ByRef rep As mlpreport)
        Try
            rep = New mlpreport()
            alglib.mlptrainlbfgs(network.csobj, xy, npoints, decay, restarts, wstep, maxits, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlptraines(ByVal network As multilayerperceptron, ByVal trnxy(,) As Double, ByVal trnsize As Integer, ByVal valxy(,) As Double, ByVal valsize As Integer, ByVal decay As Double, ByVal restarts As Integer, ByRef info As Integer, ByRef rep As mlpreport)
        Try
            rep = New mlpreport()
            alglib.mlptraines(network.csobj, trnxy, trnsize, valxy, valsize, decay, restarts, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpkfoldcvlbfgs(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByVal wstep As Double, ByVal maxits As Integer, ByVal foldscount As Integer, ByRef info As Integer, ByRef rep As mlpreport, ByRef cvrep As mlpcvreport)
        Try
            rep = New mlpreport()
            cvrep = New mlpcvreport()
            alglib.mlpkfoldcvlbfgs(network.csobj, xy, npoints, decay, restarts, wstep, maxits, foldscount, info, rep.csobj, cvrep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpkfoldcvlm(ByVal network As multilayerperceptron, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByVal foldscount As Integer, ByRef info As Integer, ByRef rep As mlpreport, ByRef cvrep As mlpcvreport)
        Try
            rep = New mlpreport()
            cvrep = New mlpcvreport()
            alglib.mlpkfoldcvlm(network.csobj, xy, npoints, decay, restarts, foldscount, info, rep.csobj, cvrep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class mlpensemble
        Public csobj As alglib.mlpensemble
    End Class


    Public Sub mlpecreate0(ByVal nin As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreate0(nin, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreate1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreate1(nin, nhid, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreate2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreate2(nin, nhid1, nhid2, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreateb0(ByVal nin As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreateb0(nin, nout, b, d, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreateb1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreateb1(nin, nhid, nout, b, d, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreateb2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal b As Double, ByVal d As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreateb2(nin, nhid1, nhid2, nout, b, d, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreater0(ByVal nin As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreater0(nin, nout, a, b, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreater1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreater1(nin, nhid, nout, a, b, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreater2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal a As Double, ByVal b As Double, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreater2(nin, nhid1, nhid2, nout, a, b, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreatec0(ByVal nin As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreatec0(nin, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreatec1(ByVal nin As Integer, ByVal nhid As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreatec1(nin, nhid, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreatec2(ByVal nin As Integer, ByVal nhid1 As Integer, ByVal nhid2 As Integer, ByVal nout As Integer, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreatec2(nin, nhid1, nhid2, nout, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpecreatefromnetwork(ByVal network As multilayerperceptron, ByVal ensemblesize As Integer, ByRef ensemble As mlpensemble)
        Try
            ensemble = New mlpensemble()
            alglib.mlpecreatefromnetwork(network.csobj, ensemblesize, ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlperandomize(ByVal ensemble As mlpensemble)
        Try
            alglib.mlperandomize(ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpeproperties(ByVal ensemble As mlpensemble, ByRef nin As Integer, ByRef nout As Integer)
        Try
            alglib.mlpeproperties(ensemble.csobj, nin, nout)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mlpeissoftmax(ByVal ensemble As mlpensemble) As Boolean
        Try
            mlpeissoftmax = alglib.mlpeissoftmax(ensemble.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub mlpeprocess(ByVal ensemble As mlpensemble, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mlpeprocess(ensemble.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpeprocessi(ByVal ensemble As mlpensemble, ByVal x() As Double, ByRef y() As Double)
        Try
            alglib.mlpeprocessi(ensemble.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mlperelclserror(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlperelclserror = alglib.mlperelclserror(ensemble.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpeavgce(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpeavgce = alglib.mlpeavgce(ensemble.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpermserror(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpermserror = alglib.mlpermserror(ensemble.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpeavgerror(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpeavgerror = alglib.mlpeavgerror(ensemble.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function mlpeavgrelerror(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer) As Double
        Try
            mlpeavgrelerror = alglib.mlpeavgrelerror(ensemble.csobj, xy, npoints)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub mlpebagginglm(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByRef info As Integer, ByRef rep As mlpreport, ByRef ooberrors As mlpcvreport)
        Try
            rep = New mlpreport()
            ooberrors = New mlpcvreport()
            alglib.mlpebagginglm(ensemble.csobj, xy, npoints, decay, restarts, info, rep.csobj, ooberrors.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpebagginglbfgs(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByVal wstep As Double, ByVal maxits As Integer, ByRef info As Integer, ByRef rep As mlpreport, ByRef ooberrors As mlpcvreport)
        Try
            rep = New mlpreport()
            ooberrors = New mlpcvreport()
            alglib.mlpebagginglbfgs(ensemble.csobj, xy, npoints, decay, restarts, wstep, maxits, info, rep.csobj, ooberrors.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mlpetraines(ByVal ensemble As mlpensemble, ByVal xy(,) As Double, ByVal npoints As Integer, ByVal decay As Double, ByVal restarts As Integer, ByRef info As Integer, ByRef rep As mlpreport)
        Try
            rep = New mlpreport()
            alglib.mlpetraines(ensemble.csobj, xy, npoints, decay, restarts, info, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub pcabuildbasis(ByVal x(,) As Double, ByVal npoints As Integer, ByVal nvars As Integer, ByRef info As Integer, ByRef s2() As Double, ByRef v(,) As Double)
        Try
            alglib.pcabuildbasis(x, npoints, nvars, info, s2, v)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class odesolverstate
        Public csobj As alglib.odesolverstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class odesolverreport
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public csobj As alglib.odesolverreport
    End Class


    Public Sub odesolverrkck(ByVal y() As Double, ByVal n As Integer, ByVal x() As Double, ByVal m As Integer, ByVal eps As Double, ByVal h As Double, ByRef state As odesolverstate)
        Try
            state = New odesolverstate()
            alglib.odesolverrkck(y, n, x, m, eps, h, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub odesolverrkck(ByVal y() As Double, ByVal x() As Double, ByVal eps As Double, ByVal h As Double, ByRef state As odesolverstate)
        Try
            state = New odesolverstate()
            alglib.odesolverrkck(y, x, eps, h, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function odesolveriteration(ByVal state As odesolverstate) As Boolean
        Try
            odesolveriteration = alglib.odesolveriteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This function is used to launcn iterations of ODE solver
    '
    ' It accepts following parameters:
    '     diff    -   callback which calculates dy/dx for given y and x
    '     obj     -   optional object which is passed to diff; can be NULL
    '
    ' 
    '   -- ALGLIB --
    '      Copyright 01.09.2009 by Bochkanov Sergey
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''/
    Public Sub odesolversolve(state As odesolverstate, diff As ndimensional_ode_rp, obj As Object)
        If diff Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'odesolversolve()' (diff is null)")
        End If
        Dim innerobj As alglib.odesolver.odesolverstate = state.csobj.innerobj
        Try
            While alglib.odesolver.odesolveriteration(innerobj)
                If innerobj.needdy Then
                    diff(innerobj.y, innerobj.x, innerobj.dy, obj)
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: unexpected error in 'odesolversolve'")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub odesolverresults(ByVal state As odesolverstate, ByRef m As Integer, ByRef xtbl() As Double, ByRef ytbl(,) As Double, ByRef rep As odesolverreport)
        Try
            rep = New odesolverreport()
            alglib.odesolverresults(state.csobj, m, xtbl, ytbl, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub fftc1d(ByRef a() As alglib.complex, ByVal n As Integer)
        Try
            alglib.fftc1d(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftc1d(ByRef a() As alglib.complex)
        Try
            alglib.fftc1d(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftc1dinv(ByRef a() As alglib.complex, ByVal n As Integer)
        Try
            alglib.fftc1dinv(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftc1dinv(ByRef a() As alglib.complex)
        Try
            alglib.fftc1dinv(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftr1d(ByVal a() As Double, ByVal n As Integer, ByRef f() As alglib.complex)
        Try
            alglib.fftr1d(a, n, f)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftr1d(ByVal a() As Double, ByRef f() As alglib.complex)
        Try
            alglib.fftr1d(a, f)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftr1dinv(ByVal f() As alglib.complex, ByVal n As Integer, ByRef a() As Double)
        Try
            alglib.fftr1dinv(f, n, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fftr1dinv(ByVal f() As alglib.complex, ByRef a() As Double)
        Try
            alglib.fftr1dinv(f, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub convc1d(ByVal a() As alglib.complex, ByVal m As Integer, ByVal b() As alglib.complex, ByVal n As Integer, ByRef r() As alglib.complex)
        Try
            alglib.convc1d(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convc1dinv(ByVal a() As alglib.complex, ByVal m As Integer, ByVal b() As alglib.complex, ByVal n As Integer, ByRef r() As alglib.complex)
        Try
            alglib.convc1dinv(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convc1dcircular(ByVal s() As alglib.complex, ByVal m As Integer, ByVal r() As alglib.complex, ByVal n As Integer, ByRef c() As alglib.complex)
        Try
            alglib.convc1dcircular(s, m, r, n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convc1dcircularinv(ByVal a() As alglib.complex, ByVal m As Integer, ByVal b() As alglib.complex, ByVal n As Integer, ByRef r() As alglib.complex)
        Try
            alglib.convc1dcircularinv(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convr1d(ByVal a() As Double, ByVal m As Integer, ByVal b() As Double, ByVal n As Integer, ByRef r() As Double)
        Try
            alglib.convr1d(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convr1dinv(ByVal a() As Double, ByVal m As Integer, ByVal b() As Double, ByVal n As Integer, ByRef r() As Double)
        Try
            alglib.convr1dinv(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convr1dcircular(ByVal s() As Double, ByVal m As Integer, ByVal r() As Double, ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.convr1dcircular(s, m, r, n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub convr1dcircularinv(ByVal a() As Double, ByVal m As Integer, ByVal b() As Double, ByVal n As Integer, ByRef r() As Double)
        Try
            alglib.convr1dcircularinv(a, m, b, n, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub corrc1d(ByVal signal() As alglib.complex, ByVal n As Integer, ByVal pattern() As alglib.complex, ByVal m As Integer, ByRef r() As alglib.complex)
        Try
            alglib.corrc1d(signal, n, pattern, m, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub corrc1dcircular(ByVal signal() As alglib.complex, ByVal m As Integer, ByVal pattern() As alglib.complex, ByVal n As Integer, ByRef c() As alglib.complex)
        Try
            alglib.corrc1dcircular(signal, m, pattern, n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub corrr1d(ByVal signal() As Double, ByVal n As Integer, ByVal pattern() As Double, ByVal m As Integer, ByRef r() As Double)
        Try
            alglib.corrr1d(signal, n, pattern, m, r)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub corrr1dcircular(ByVal signal() As Double, ByVal m As Integer, ByVal pattern() As Double, ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.corrr1dcircular(signal, m, pattern, n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub fhtr1d(ByRef a() As Double, ByVal n As Integer)
        Try
            alglib.fhtr1d(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fhtr1dinv(ByRef a() As Double, ByVal n As Integer)
        Try
            alglib.fhtr1dinv(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub gqgeneraterec(ByVal alpha() As Double, ByVal beta() As Double, ByVal mu0 As Double, ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgeneraterec(alpha, beta, mu0, n, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategausslobattorec(ByVal alpha() As Double, ByVal beta() As Double, ByVal mu0 As Double, ByVal a As Double, ByVal b As Double, ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategausslobattorec(alpha, beta, mu0, a, b, n, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategaussradaurec(ByVal alpha() As Double, ByVal beta() As Double, ByVal mu0 As Double, ByVal a As Double, ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategaussradaurec(alpha, beta, mu0, a, n, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategausslegendre(ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategausslegendre(n, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategaussjacobi(ByVal n As Integer, ByVal alpha As Double, ByVal beta As Double, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategaussjacobi(n, alpha, beta, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategausslaguerre(ByVal n As Integer, ByVal alpha As Double, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategausslaguerre(n, alpha, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gqgenerategausshermite(ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef w() As Double)
        Try
            alglib.gqgenerategausshermite(n, info, x, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub gkqgeneraterec(ByVal alpha() As Double, ByVal beta() As Double, ByVal mu0 As Double, ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef wkronrod() As Double, ByRef wgauss() As Double)
        Try
            alglib.gkqgeneraterec(alpha, beta, mu0, n, info, x, wkronrod, wgauss)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gkqgenerategausslegendre(ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef wkronrod() As Double, ByRef wgauss() As Double)
        Try
            alglib.gkqgenerategausslegendre(n, info, x, wkronrod, wgauss)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gkqgenerategaussjacobi(ByVal n As Integer, ByVal alpha As Double, ByVal beta As Double, ByRef info As Integer, ByRef x() As Double, ByRef wkronrod() As Double, ByRef wgauss() As Double)
        Try
            alglib.gkqgenerategaussjacobi(n, alpha, beta, info, x, wkronrod, wgauss)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gkqlegendrecalc(ByVal n As Integer, ByRef info As Integer, ByRef x() As Double, ByRef wkronrod() As Double, ByRef wgauss() As Double)
        Try
            alglib.gkqlegendrecalc(n, info, x, wkronrod, wgauss)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub gkqlegendretbl(ByVal n As Integer, ByRef x() As Double, ByRef wkronrod() As Double, ByRef wgauss() As Double, ByRef eps As Double)
        Try
            alglib.gkqlegendretbl(n, x, wkronrod, wgauss, eps)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Integration report:
    '* TerminationType = completetion code:
    '    * -5    non-convergence of Gauss-Kronrod nodes
    '            calculation subroutine.
    '    * -1    incorrect parameters were specified
    '    *  1    OK
    '* Rep.NFEV countains number of function calculations
    '* Rep.NIntervals contains number of intervals [a,b]
    '  was partitioned into.
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class autogkreport
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property nintervals() As Integer
        Get
            Return csobj.nintervals
        End Get
        Set(ByVal Value As Integer)
            csobj.nintervals = Value
        End Set
        End Property
        Public csobj As alglib.autogkreport
    End Class
    Public Class autogkstate
        Public csobj As alglib.autogkstate
    End Class


    Public Sub autogksmooth(ByVal a As Double, ByVal b As Double, ByRef state As autogkstate)
        Try
            state = New autogkstate()
            alglib.autogksmooth(a, b, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub autogksmoothw(ByVal a As Double, ByVal b As Double, ByVal xwidth As Double, ByRef state As autogkstate)
        Try
            state = New autogkstate()
            alglib.autogksmoothw(a, b, xwidth, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub autogksingular(ByVal a As Double, ByVal b As Double, ByVal alpha As Double, ByVal beta As Double, ByRef state As autogkstate)
        Try
            state = New autogkstate()
            alglib.autogksingular(a, b, alpha, beta, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function autogkiteration(ByVal state As autogkstate) As Boolean
        Try
            autogkiteration = alglib.autogkiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This function is used to launcn iterations of ODE solver
    '
    ' It accepts following parameters:
    '     diff    -   callback which calculates dy/dx for given y and x
    '     obj     -   optional object which is passed to diff; can be NULL
    '
    ' 
    '   -- ALGLIB --
    '      Copyright 07.05.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub autogkintegrate(state As autogkstate, func As integrator1_func, obj As Object)
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'autogkintegrate()' (func is null)")
        End If
        Dim innerobj As alglib.autogk.autogkstate = state.csobj.innerobj
        Try
            While alglib.autogk.autogkiteration(innerobj)
                If innerobj.needf Then
                    func(innerobj.x, innerobj.xminusa, innerobj.bminusx, innerobj.f, obj)
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: unexpected error in 'autogksolve'")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub autogkresults(ByVal state As autogkstate, ByRef v As Double, ByRef rep As autogkreport)
        Try
            rep = New autogkreport()
            alglib.autogkresults(state.csobj, v, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class idwinterpolant
        Public csobj As alglib.idwinterpolant
    End Class


    Public Function idwcalc(ByVal z As idwinterpolant, ByVal x() As Double) As Double
        Try
            idwcalc = alglib.idwcalc(z.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub idwbuildmodifiedshepard(ByVal xy(,) As Double, ByVal n As Integer, ByVal nx As Integer, ByVal d As Integer, ByVal nq As Integer, ByVal nw As Integer, ByRef z As idwinterpolant)
        Try
            z = New idwinterpolant()
            alglib.idwbuildmodifiedshepard(xy, n, nx, d, nq, nw, z.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub idwbuildmodifiedshepardr(ByVal xy(,) As Double, ByVal n As Integer, ByVal nx As Integer, ByVal r As Double, ByRef z As idwinterpolant)
        Try
            z = New idwinterpolant()
            alglib.idwbuildmodifiedshepardr(xy, n, nx, r, z.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub idwbuildnoisy(ByVal xy(,) As Double, ByVal n As Integer, ByVal nx As Integer, ByVal d As Integer, ByVal nq As Integer, ByVal nw As Integer, ByRef z As idwinterpolant)
        Try
            z = New idwinterpolant()
            alglib.idwbuildnoisy(xy, n, nx, d, nq, nw, z.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class barycentricinterpolant
        Public csobj As alglib.barycentricinterpolant
    End Class


    Public Function barycentriccalc(ByVal b As barycentricinterpolant, ByVal t As Double) As Double
        Try
            barycentriccalc = alglib.barycentriccalc(b.csobj, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub barycentricdiff1(ByVal b As barycentricinterpolant, ByVal t As Double, ByRef f As Double, ByRef df As Double)
        Try
            alglib.barycentricdiff1(b.csobj, t, f, df)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricdiff2(ByVal b As barycentricinterpolant, ByVal t As Double, ByRef f As Double, ByRef df As Double, ByRef d2f As Double)
        Try
            alglib.barycentricdiff2(b.csobj, t, f, df, d2f)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentriclintransx(ByVal b As barycentricinterpolant, ByVal ca As Double, ByVal cb As Double)
        Try
            alglib.barycentriclintransx(b.csobj, ca, cb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentriclintransy(ByVal b As barycentricinterpolant, ByVal ca As Double, ByVal cb As Double)
        Try
            alglib.barycentriclintransy(b.csobj, ca, cb)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricunpack(ByVal b As barycentricinterpolant, ByRef n As Integer, ByRef x() As Double, ByRef y() As Double, ByRef w() As Double)
        Try
            alglib.barycentricunpack(b.csobj, n, x, y, w)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricbuildxyw(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByRef b As barycentricinterpolant)
        Try
            b = New barycentricinterpolant()
            alglib.barycentricbuildxyw(x, y, w, n, b.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricbuildfloaterhormann(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal d As Integer, ByRef b As barycentricinterpolant)
        Try
            b = New barycentricinterpolant()
            alglib.barycentricbuildfloaterhormann(x, y, n, d, b.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub polynomialbar2cheb(ByVal p As barycentricinterpolant, ByVal a As Double, ByVal b As Double, ByRef t() As Double)
        Try
            alglib.polynomialbar2cheb(p.csobj, a, b, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialcheb2bar(ByVal t() As Double, ByVal n As Integer, ByVal a As Double, ByVal b As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialcheb2bar(t, n, a, b, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialcheb2bar(ByVal t() As Double, ByVal a As Double, ByVal b As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialcheb2bar(t, a, b, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbar2pow(ByVal p As barycentricinterpolant, ByVal c As Double, ByVal s As Double, ByRef a() As Double)
        Try
            alglib.polynomialbar2pow(p.csobj, c, s, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbar2pow(ByVal p As barycentricinterpolant, ByRef a() As Double)
        Try
            alglib.polynomialbar2pow(p.csobj, a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialpow2bar(ByVal a() As Double, ByVal n As Integer, ByVal c As Double, ByVal s As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialpow2bar(a, n, c, s, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialpow2bar(ByVal a() As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialpow2bar(a, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuild(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuild(x, y, n, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuild(ByVal x() As Double, ByVal y() As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuild(x, y, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildeqdist(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByVal n As Integer, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildeqdist(a, b, y, n, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildeqdist(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildeqdist(a, b, y, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildcheb1(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByVal n As Integer, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildcheb1(a, b, y, n, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildcheb1(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildcheb1(a, b, y, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildcheb2(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByVal n As Integer, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildcheb2(a, b, y, n, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialbuildcheb2(ByVal a As Double, ByVal b As Double, ByVal y() As Double, ByRef p As barycentricinterpolant)
        Try
            p = New barycentricinterpolant()
            alglib.polynomialbuildcheb2(a, b, y, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function polynomialcalceqdist(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal n As Integer, ByVal t As Double) As Double
        Try
            polynomialcalceqdist = alglib.polynomialcalceqdist(a, b, f, n, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function polynomialcalceqdist(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal t As Double) As Double
        Try
            polynomialcalceqdist = alglib.polynomialcalceqdist(a, b, f, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function polynomialcalccheb1(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal n As Integer, ByVal t As Double) As Double
        Try
            polynomialcalccheb1 = alglib.polynomialcalccheb1(a, b, f, n, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function polynomialcalccheb1(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal t As Double) As Double
        Try
            polynomialcalccheb1 = alglib.polynomialcalccheb1(a, b, f, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function polynomialcalccheb2(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal n As Integer, ByVal t As Double) As Double
        Try
            polynomialcalccheb2 = alglib.polynomialcalccheb2(a, b, f, n, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function polynomialcalccheb2(ByVal a As Double, ByVal b As Double, ByVal f() As Double, ByVal t As Double) As Double
        Try
            polynomialcalccheb2 = alglib.polynomialcalccheb2(a, b, f, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class spline1dinterpolant
        Public csobj As alglib.spline1dinterpolant
    End Class


    Public Sub spline1dbuildlinear(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildlinear(x, y, n, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildlinear(ByVal x() As Double, ByVal y() As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildlinear(x, y, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildcubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildcubic(x, y, n, boundltype, boundl, boundrtype, boundr, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildcubic(ByVal x() As Double, ByVal y() As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildcubic(x, y, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dgriddiffcubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByRef d() As Double)
        Try
            alglib.spline1dgriddiffcubic(x, y, n, boundltype, boundl, boundrtype, boundr, d)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dgriddiffcubic(ByVal x() As Double, ByVal y() As Double, ByRef d() As Double)
        Try
            alglib.spline1dgriddiffcubic(x, y, d)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dgriddiff2cubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByRef d1() As Double, ByRef d2() As Double)
        Try
            alglib.spline1dgriddiff2cubic(x, y, n, boundltype, boundl, boundrtype, boundr, d1, d2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dgriddiff2cubic(ByVal x() As Double, ByVal y() As Double, ByRef d1() As Double, ByRef d2() As Double)
        Try
            alglib.spline1dgriddiff2cubic(x, y, d1, d2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvcubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByVal x2() As Double, ByVal n2 As Integer, ByRef y2() As Double)
        Try
            alglib.spline1dconvcubic(x, y, n, boundltype, boundl, boundrtype, boundr, x2, n2, y2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvcubic(ByVal x() As Double, ByVal y() As Double, ByVal x2() As Double, ByRef y2() As Double)
        Try
            alglib.spline1dconvcubic(x, y, x2, y2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvdiffcubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByVal x2() As Double, ByVal n2 As Integer, ByRef y2() As Double, ByRef d2() As Double)
        Try
            alglib.spline1dconvdiffcubic(x, y, n, boundltype, boundl, boundrtype, boundr, x2, n2, y2, d2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvdiffcubic(ByVal x() As Double, ByVal y() As Double, ByVal x2() As Double, ByRef y2() As Double, ByRef d2() As Double)
        Try
            alglib.spline1dconvdiffcubic(x, y, x2, y2, d2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvdiff2cubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundltype As Integer, ByVal boundl As Double, ByVal boundrtype As Integer, ByVal boundr As Double, ByVal x2() As Double, ByVal n2 As Integer, ByRef y2() As Double, ByRef d2() As Double, ByRef dd2() As Double)
        Try
            alglib.spline1dconvdiff2cubic(x, y, n, boundltype, boundl, boundrtype, boundr, x2, n2, y2, d2, dd2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dconvdiff2cubic(ByVal x() As Double, ByVal y() As Double, ByVal x2() As Double, ByRef y2() As Double, ByRef d2() As Double, ByRef dd2() As Double)
        Try
            alglib.spline1dconvdiff2cubic(x, y, x2, y2, d2, dd2)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildcatmullrom(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal boundtype As Integer, ByVal tension As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildcatmullrom(x, y, n, boundtype, tension, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildcatmullrom(ByVal x() As Double, ByVal y() As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildcatmullrom(x, y, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildhermite(ByVal x() As Double, ByVal y() As Double, ByVal d() As Double, ByVal n As Integer, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildhermite(x, y, d, n, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildhermite(ByVal x() As Double, ByVal y() As Double, ByVal d() As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildhermite(x, y, d, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildakima(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildakima(x, y, n, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dbuildakima(ByVal x() As Double, ByVal y() As Double, ByRef c As spline1dinterpolant)
        Try
            c = New spline1dinterpolant()
            alglib.spline1dbuildakima(x, y, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function spline1dcalc(ByVal c As spline1dinterpolant, ByVal x As Double) As Double
        Try
            spline1dcalc = alglib.spline1dcalc(c.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub spline1ddiff(ByVal c As spline1dinterpolant, ByVal x As Double, ByRef s As Double, ByRef ds As Double, ByRef d2s As Double)
        Try
            alglib.spline1ddiff(c.csobj, x, s, ds, d2s)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dunpack(ByVal c As spline1dinterpolant, ByRef n As Integer, ByRef tbl(,) As Double)
        Try
            alglib.spline1dunpack(c.csobj, n, tbl)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dlintransx(ByVal c As spline1dinterpolant, ByVal a As Double, ByVal b As Double)
        Try
            alglib.spline1dlintransx(c.csobj, a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dlintransy(ByVal c As spline1dinterpolant, ByVal a As Double, ByVal b As Double)
        Try
            alglib.spline1dlintransy(c.csobj, a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function spline1dintegrate(ByVal c As spline1dinterpolant, ByVal x As Double) As Double
        Try
            spline1dintegrate = alglib.spline1dintegrate(c.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class minlmstate
        Public csobj As alglib.minlmstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Optimization report, filled by MinLMResults() function
    '
    'FIELDS:
    '* TerminationType, completetion code:
    '    * -9    derivative correctness check failed;
    '            see Rep.WrongNum, Rep.WrongI, Rep.WrongJ for
    '            more information.
    '    *  1    relative function improvement is no more than
    '            EpsF.
    '    *  2    relative step is no more than EpsX.
    '    *  4    gradient is no more than EpsG.
    '    *  5    MaxIts steps was taken
    '    *  7    stopping conditions are too stringent,
    '            further improvement is impossible
    '* IterationsCount, contains iterations count
    '* NFunc, number of function calculations
    '* NJac, number of Jacobi matrix calculations
    '* NGrad, number of gradient calculations
    '* NHess, number of Hessian calculations
    '* NCholesky, number of Cholesky decomposition calculations
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class minlmreport
        Public Property iterationscount() As Integer
        Get
            Return csobj.iterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.iterationscount = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public Property nfunc() As Integer
        Get
            Return csobj.nfunc
        End Get
        Set(ByVal Value As Integer)
            csobj.nfunc = Value
        End Set
        End Property
        Public Property njac() As Integer
        Get
            Return csobj.njac
        End Get
        Set(ByVal Value As Integer)
            csobj.njac = Value
        End Set
        End Property
        Public Property ngrad() As Integer
        Get
            Return csobj.ngrad
        End Get
        Set(ByVal Value As Integer)
            csobj.ngrad = Value
        End Set
        End Property
        Public Property nhess() As Integer
        Get
            Return csobj.nhess
        End Get
        Set(ByVal Value As Integer)
            csobj.nhess = Value
        End Set
        End Property
        Public Property ncholesky() As Integer
        Get
            Return csobj.ncholesky
        End Get
        Set(ByVal Value As Integer)
            csobj.ncholesky = Value
        End Set
        End Property
        Public csobj As alglib.minlmreport
    End Class


    Public Sub minlmcreatevj(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatevj(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatevj(ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatevj(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatev(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByVal diffstep As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatev(n, m, x, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatev(ByVal m As Integer, ByVal x() As Double, ByVal diffstep As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatev(m, x, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefgh(ByVal n As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefgh(n, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefgh(ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefgh(x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatevgj(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatevgj(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatevgj(ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatevgj(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefgj(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefgj(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefgj(ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefgj(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefj(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefj(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmcreatefj(ByVal m As Integer, ByVal x() As Double, ByRef state As minlmstate)
        Try
            state = New minlmstate()
            alglib.minlmcreatefj(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmsetcond(ByVal state As minlmstate, ByVal epsg As Double, ByVal epsf As Double, ByVal epsx As Double, ByVal maxits As Integer)
        Try
            alglib.minlmsetcond(state.csobj, epsg, epsf, epsx, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmsetxrep(ByVal state As minlmstate, ByVal needxrep As Boolean)
        Try
            alglib.minlmsetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmsetstpmax(ByVal state As minlmstate, ByVal stpmax As Double)
        Try
            alglib.minlmsetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmsetacctype(ByVal state As minlmstate, ByVal acctype As Integer)
        Try
            alglib.minlmsetacctype(state.csobj, acctype)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function minlmiteration(ByVal state As minlmstate) As Boolean
        Try
            minlmiteration = alglib.minlmiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear optimizer
    ' 
    ' These functions accept following parameters:
    '     func    -   callback which calculates function (or merit function)
    '                 value func at given point x
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     hess    -   callback which calculates function (or merit function)
    '                 value func, gradient grad and Hessian hess at given point x
    '     fvec    -   callback which calculates function vector fi[]
    '                 at given point x
    '     jac     -   callback which calculates function vector fi[]
    '                 and Jacobian jac at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' NOTES:
    ' 
    ' 1. Depending on function used to create state  structure,  this  algorithm
    '    may accept Jacobian and/or Hessian and/or gradient.  According  to  the
    '    said above, there ase several versions of this function,  which  accept
    '    different sets of callbacks.
    ' 
    '    This flexibility opens way to subtle errors - you may create state with
    '    MinLMCreateFGH() (optimization using Hessian), but call function  which
    '    does not accept Hessian. So when algorithm will request Hessian,  there
    '    will be no callback to call. In this case exception will be thrown.
    ' 
    '    Be careful to avoid such errors because there is no way to find them at
    '    compile time - you can see them at runtime only.
    ' 
    '   -- ALGLIB --
    '      Copyright 10.03.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub minlmoptimize(ByVal state As minlmstate, ByVal fvec As ndimensional_fvec, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlm.minlmstate = state.csobj.innerobj
        If fvec Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (fvec is null)")
        End If
        Try
            While alglib.minlm.minlmiteration(innerobj)
                If innerobj.needfi Then
                    fvec(innerobj.x, innerobj.fi, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlmoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub minlmoptimize(ByVal state As minlmstate, ByVal fvec As ndimensional_fvec, ByVal jac As ndimensional_jac, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlm.minlmstate = state.csobj.innerobj
        If fvec Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (fvec is null)")
        End If
        If jac Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (jac is null)")
        End If
        Try
            While alglib.minlm.minlmiteration(innerobj)
                If innerobj.needfi Then
                    fvec(innerobj.x, innerobj.fi, obj)
                    Continue While
                End If
                If innerobj.needfij Then
                    jac(innerobj.x, innerobj.fi, innerobj.j, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlmoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub minlmoptimize(ByVal state As minlmstate, ByVal func As ndimensional_func, ByVal grad As ndimensional_grad, ByVal hess As ndimensional_hess, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlm.minlmstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (func is null)")
        End If
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (grad is null)")
        End If
        If hess Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (hess is null)")
        End If
        Try
            While alglib.minlm.minlmiteration(innerobj)
                If innerobj.needf Then
                    func(innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.needfgh Then
                    hess(innerobj.x, innerobj.f, innerobj.g, innerobj.h, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlmoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub minlmoptimize(ByVal state As minlmstate, ByVal func As ndimensional_func, ByVal jac As ndimensional_jac, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlm.minlmstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (func is null)")
        End If
        If jac Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (jac is null)")
        End If
        Try
            While alglib.minlm.minlmiteration(innerobj)
                If innerobj.needf Then
                    func(innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfij Then
                    jac(innerobj.x, innerobj.fi, innerobj.j, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlmoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub minlmoptimize(ByVal state As minlmstate, ByVal func As ndimensional_func, ByVal grad As ndimensional_grad, ByVal jac As ndimensional_jac, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minlm.minlmstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (func is null)")
        End If
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (grad is null)")
        End If
        If jac Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minlmoptimize()' (jac is null)")
        End If
        Try
            While alglib.minlm.minlmiteration(innerobj)
                If innerobj.needf Then
                    func(innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.needfij Then
                    jac(innerobj.x, innerobj.fi, innerobj.j, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minlmoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub minlmresults(ByVal state As minlmstate, ByRef x() As Double, ByRef rep As minlmreport)
        Try
            rep = New minlmreport()
            alglib.minlmresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmresultsbuf(ByVal state As minlmstate, ByRef x() As Double, ByRef rep As minlmreport)
        Try
            alglib.minlmresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minlmrestartfrom(ByVal state As minlmstate, ByVal x() As Double)
        Try
            alglib.minlmrestartfrom(state.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Polynomial fitting report:
    '    TaskRCond       reciprocal of task's condition number
    '    RMSError        RMS error
    '    AvgError        average error
    '    AvgRelError     average relative error (for non-zero Y[I])
    '    MaxError        maximum error
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class polynomialfitreport
        Public Property taskrcond() As Double
        Get
            Return csobj.taskrcond
        End Get
        Set(ByVal Value As Double)
            csobj.taskrcond = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property maxerror() As Double
        Get
            Return csobj.maxerror
        End Get
        Set(ByVal Value As Double)
            csobj.maxerror = Value
        End Set
        End Property
        Public csobj As alglib.polynomialfitreport
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Barycentric fitting report:
    '    RMSError        RMS error
    '    AvgError        average error
    '    AvgRelError     average relative error (for non-zero Y[I])
    '    MaxError        maximum error
    '    TaskRCond       reciprocal of task's condition number
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class barycentricfitreport
        Public Property taskrcond() As Double
        Get
            Return csobj.taskrcond
        End Get
        Set(ByVal Value As Double)
            csobj.taskrcond = Value
        End Set
        End Property
        Public Property dbest() As Integer
        Get
            Return csobj.dbest
        End Get
        Set(ByVal Value As Integer)
            csobj.dbest = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property maxerror() As Double
        Get
            Return csobj.maxerror
        End Get
        Set(ByVal Value As Double)
            csobj.maxerror = Value
        End Set
        End Property
        Public csobj As alglib.barycentricfitreport
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Spline fitting report:
    '    RMSError        RMS error
    '    AvgError        average error
    '    AvgRelError     average relative error (for non-zero Y[I])
    '    MaxError        maximum error
    '
    'Fields  below are  filled  by   obsolete    functions   (Spline1DFitCubic,
    'Spline1DFitHermite). Modern fitting functions do NOT fill these fields:
    '    TaskRCond       reciprocal of task's condition number
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class spline1dfitreport
        Public Property taskrcond() As Double
        Get
            Return csobj.taskrcond
        End Get
        Set(ByVal Value As Double)
            csobj.taskrcond = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property maxerror() As Double
        Get
            Return csobj.maxerror
        End Get
        Set(ByVal Value As Double)
            csobj.maxerror = Value
        End Set
        End Property
        Public csobj As alglib.spline1dfitreport
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'Least squares fitting report:
    '    TaskRCond       reciprocal of task's condition number
    '    RMSError        RMS error
    '    AvgError        average error
    '    AvgRelError     average relative error (for non-zero Y[I])
    '    MaxError        maximum error
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class lsfitreport
        Public Property taskrcond() As Double
        Get
            Return csobj.taskrcond
        End Get
        Set(ByVal Value As Double)
            csobj.taskrcond = Value
        End Set
        End Property
        Public Property rmserror() As Double
        Get
            Return csobj.rmserror
        End Get
        Set(ByVal Value As Double)
            csobj.rmserror = Value
        End Set
        End Property
        Public Property avgerror() As Double
        Get
            Return csobj.avgerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgerror = Value
        End Set
        End Property
        Public Property avgrelerror() As Double
        Get
            Return csobj.avgrelerror
        End Get
        Set(ByVal Value As Double)
            csobj.avgrelerror = Value
        End Set
        End Property
        Public Property maxerror() As Double
        Get
            Return csobj.maxerror
        End Get
        Set(ByVal Value As Double)
            csobj.maxerror = Value
        End Set
        End Property
        Public csobj As alglib.lsfitreport
    End Class
    Public Class lsfitstate
        Public csobj As alglib.lsfitstate
    End Class


    Public Sub polynomialfit(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef p As barycentricinterpolant, ByRef rep As polynomialfitreport)
        Try
            p = New barycentricinterpolant()
            rep = New polynomialfitreport()
            alglib.polynomialfit(x, y, n, m, info, p.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialfit(ByVal x() As Double, ByVal y() As Double, ByVal m As Integer, ByRef info As Integer, ByRef p As barycentricinterpolant, ByRef rep As polynomialfitreport)
        Try
            p = New barycentricinterpolant()
            rep = New polynomialfitreport()
            alglib.polynomialfit(x, y, m, info, p.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialfitwc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal k As Integer, ByVal m As Integer, ByRef info As Integer, ByRef p As barycentricinterpolant, ByRef rep As polynomialfitreport)
        Try
            p = New barycentricinterpolant()
            rep = New polynomialfitreport()
            alglib.polynomialfitwc(x, y, w, n, xc, yc, dc, k, m, info, p.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub polynomialfitwc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal m As Integer, ByRef info As Integer, ByRef p As barycentricinterpolant, ByRef rep As polynomialfitreport)
        Try
            p = New barycentricinterpolant()
            rep = New polynomialfitreport()
            alglib.polynomialfitwc(x, y, w, xc, yc, dc, m, info, p.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricfitfloaterhormannwc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal k As Integer, ByVal m As Integer, ByRef info As Integer, ByRef b As barycentricinterpolant, ByRef rep As barycentricfitreport)
        Try
            b = New barycentricinterpolant()
            rep = New barycentricfitreport()
            alglib.barycentricfitfloaterhormannwc(x, y, w, n, xc, yc, dc, k, m, info, b.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub barycentricfitfloaterhormann(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef b As barycentricinterpolant, ByRef rep As barycentricfitreport)
        Try
            b = New barycentricinterpolant()
            rep = New barycentricfitreport()
            alglib.barycentricfitfloaterhormann(x, y, n, m, info, b.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitpenalized(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal m As Integer, ByVal rho As Double, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitpenalized(x, y, n, m, rho, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitpenalized(ByVal x() As Double, ByVal y() As Double, ByVal m As Integer, ByVal rho As Double, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitpenalized(x, y, m, rho, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitpenalizedw(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByVal m As Integer, ByVal rho As Double, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitpenalizedw(x, y, w, n, m, rho, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitpenalizedw(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal m As Integer, ByVal rho As Double, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitpenalizedw(x, y, w, m, rho, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitcubicwc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal k As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitcubicwc(x, y, w, n, xc, yc, dc, k, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitcubicwc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitcubicwc(x, y, w, xc, yc, dc, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfithermitewc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal n As Integer, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal k As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfithermitewc(x, y, w, n, xc, yc, dc, k, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfithermitewc(ByVal x() As Double, ByVal y() As Double, ByVal w() As Double, ByVal xc() As Double, ByVal yc() As Double, ByVal dc() As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfithermitewc(x, y, w, xc, yc, dc, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitcubic(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitcubic(x, y, n, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfitcubic(ByVal x() As Double, ByVal y() As Double, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfitcubic(x, y, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfithermite(ByVal x() As Double, ByVal y() As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfithermite(x, y, n, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline1dfithermite(ByVal x() As Double, ByVal y() As Double, ByVal m As Integer, ByRef info As Integer, ByRef s As spline1dinterpolant, ByRef rep As spline1dfitreport)
        Try
            s = New spline1dinterpolant()
            rep = New spline1dfitreport()
            alglib.spline1dfithermite(x, y, m, info, s.csobj, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearw(ByVal y() As Double, ByVal w() As Double, ByVal fmatrix(,) As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearw(y, w, fmatrix, n, m, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearw(ByVal y() As Double, ByVal w() As Double, ByVal fmatrix(,) As Double, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearw(y, w, fmatrix, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearwc(ByVal y() As Double, ByVal w() As Double, ByVal fmatrix(,) As Double, ByVal cmatrix(,) As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearwc(y, w, fmatrix, cmatrix, n, m, k, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearwc(ByVal y() As Double, ByVal w() As Double, ByVal fmatrix(,) As Double, ByVal cmatrix(,) As Double, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearwc(y, w, fmatrix, cmatrix, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinear(ByVal y() As Double, ByVal fmatrix(,) As Double, ByVal n As Integer, ByVal m As Integer, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinear(y, fmatrix, n, m, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinear(ByVal y() As Double, ByVal fmatrix(,) As Double, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinear(y, fmatrix, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearc(ByVal y() As Double, ByVal fmatrix(,) As Double, ByVal cmatrix(,) As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearc(y, fmatrix, cmatrix, n, m, k, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitlinearc(ByVal y() As Double, ByVal fmatrix(,) As Double, ByVal cmatrix(,) As Double, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitlinearc(y, fmatrix, cmatrix, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewf(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByVal diffstep As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewf(x, y, w, c, n, m, k, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewf(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByVal diffstep As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewf(x, y, w, c, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatef(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByVal diffstep As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatef(x, y, c, n, m, k, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatef(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByVal diffstep As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatef(x, y, c, diffstep, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewfg(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByVal cheapfg As Boolean, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewfg(x, y, w, c, n, m, k, cheapfg, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewfg(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByVal cheapfg As Boolean, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewfg(x, y, w, c, cheapfg, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatefg(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByVal cheapfg As Boolean, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatefg(x, y, c, n, m, k, cheapfg, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatefg(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByVal cheapfg As Boolean, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatefg(x, y, c, cheapfg, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewfgh(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewfgh(x, y, w, c, n, m, k, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatewfgh(ByVal x(,) As Double, ByVal y() As Double, ByVal w() As Double, ByVal c() As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatewfgh(x, y, w, c, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatefgh(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByVal n As Integer, ByVal m As Integer, ByVal k As Integer, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatefgh(x, y, c, n, m, k, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitcreatefgh(ByVal x(,) As Double, ByVal y() As Double, ByVal c() As Double, ByRef state As lsfitstate)
        Try
            state = New lsfitstate()
            alglib.lsfitcreatefgh(x, y, c, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitsetcond(ByVal state As lsfitstate, ByVal epsf As Double, ByVal epsx As Double, ByVal maxits As Integer)
        Try
            alglib.lsfitsetcond(state.csobj, epsf, epsx, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitsetstpmax(ByVal state As lsfitstate, ByVal stpmax As Double)
        Try
            alglib.lsfitsetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub lsfitsetxrep(ByVal state As lsfitstate, ByVal needxrep As Boolean)
        Try
            alglib.lsfitsetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function lsfititeration(ByVal state As lsfitstate) As Boolean
        Try
            lsfititeration = alglib.lsfititeration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear fitter
    ' 
    ' These functions accept following parameters:
    '     func    -   callback which calculates function (or merit function)
    '                 value func at given point x
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     hess    -   callback which calculates function (or merit function)
    '                 value func, gradient grad and Hessian hess at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' NOTES:
    ' 
    ' 1. this algorithm is somewhat unusual because it works with  parameterized
    '    function f(C,X), where X is a function argument (we  have  many  points
    '    which are characterized by different  argument  values),  and  C  is  a
    '    parameter to fit.
    ' 
    '    For example, if we want to do linear fit by f(c0,c1,x) = c0*x+c1,  then
    '    x will be argument, and {c0,c1} will be parameters.
    ' 
    '    It is important to understand that this algorithm finds minimum in  the
    '    space of function PARAMETERS (not arguments), so it  needs  derivatives
    '    of f() with respect to C, not X.
    ' 
    '    In the example above it will need f=c0*x+c1 and {df/dc0,df/dc1} = {x,1}
    '    instead of {df/dx} = {c0}.
    ' 
    ' 2. Callback functions accept C as the first parameter, and X as the second
    ' 
    ' 3. If  state  was  created  with  LSFitCreateFG(),  algorithm  needs  just
    '    function   and   its   gradient,   but   if   state   was  created with
    '    LSFitCreateFGH(), algorithm will need function, gradient and Hessian.
    ' 
    '    According  to  the  said  above,  there  ase  several  versions of this
    '    function, which accept different sets of callbacks.
    ' 
    '    This flexibility opens way to subtle errors - you may create state with
    '    LSFitCreateFGH() (optimization using Hessian), but call function  which
    '    does not accept Hessian. So when algorithm will request Hessian,  there
    '    will be no callback to call. In this case exception will be thrown.
    ' 
    '    Be careful to avoid such errors because there is no way to find them at
    '    compile time - you can see them at runtime only.
    ' 
    '   -- ALGLIB --
    '      Copyright 17.08.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub lsfitfit(ByVal state As lsfitstate, ByVal func As ndimensional_pfunc, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.lsfit.lsfitstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (func is null)")
        End If
        Try
            While alglib.lsfit.lsfititeration(innerobj)
                If innerobj.needf Then
                    func(innerobj.c, innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.c, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'lsfitfit' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub lsfitfit(ByVal state As lsfitstate, ByVal func As ndimensional_pfunc, ByVal grad As ndimensional_pgrad, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.lsfit.lsfitstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (func is null)")
        End If
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (grad is null)")
        End If
        Try
            While alglib.lsfit.lsfititeration(innerobj)
                If innerobj.needf Then
                    func(innerobj.c, innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfg Then
                    grad(innerobj.c, innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.c, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'lsfitfit' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub


    Public Sub lsfitfit(ByVal state As lsfitstate, ByVal func As ndimensional_pfunc, ByVal grad As ndimensional_pgrad, ByVal hess As ndimensional_phess, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.lsfit.lsfitstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (func is null)")
        End If
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (grad is null)")
        End If
        If hess Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'lsfitfit()' (hess is null)")
        End If
        Try
            While alglib.lsfit.lsfititeration(innerobj)
                If innerobj.needf Then
                    func(innerobj.c, innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfg Then
                    grad(innerobj.c, innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.needfgh Then
                    hess(innerobj.c, innerobj.x, innerobj.f, innerobj.g, innerobj.h, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.c, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'lsfitfit' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub lsfitresults(ByVal state As lsfitstate, ByRef info As Integer, ByRef c() As Double, ByRef rep As lsfitreport)
        Try
            rep = New lsfitreport()
            alglib.lsfitresults(state.csobj, info, c, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class pspline2interpolant
        Public csobj As alglib.pspline2interpolant
    End Class
    Public Class pspline3interpolant
        Public csobj As alglib.pspline3interpolant
    End Class


    Public Sub pspline2build(ByVal xy(,) As Double, ByVal n As Integer, ByVal st As Integer, ByVal pt As Integer, ByRef p As pspline2interpolant)
        Try
            p = New pspline2interpolant()
            alglib.pspline2build(xy, n, st, pt, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3build(ByVal xy(,) As Double, ByVal n As Integer, ByVal st As Integer, ByVal pt As Integer, ByRef p As pspline3interpolant)
        Try
            p = New pspline3interpolant()
            alglib.pspline3build(xy, n, st, pt, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2buildperiodic(ByVal xy(,) As Double, ByVal n As Integer, ByVal st As Integer, ByVal pt As Integer, ByRef p As pspline2interpolant)
        Try
            p = New pspline2interpolant()
            alglib.pspline2buildperiodic(xy, n, st, pt, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3buildperiodic(ByVal xy(,) As Double, ByVal n As Integer, ByVal st As Integer, ByVal pt As Integer, ByRef p As pspline3interpolant)
        Try
            p = New pspline3interpolant()
            alglib.pspline3buildperiodic(xy, n, st, pt, p.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2parametervalues(ByVal p As pspline2interpolant, ByRef n As Integer, ByRef t() As Double)
        Try
            alglib.pspline2parametervalues(p.csobj, n, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3parametervalues(ByVal p As pspline3interpolant, ByRef n As Integer, ByRef t() As Double)
        Try
            alglib.pspline3parametervalues(p.csobj, n, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2calc(ByVal p As pspline2interpolant, ByVal t As Double, ByRef x As Double, ByRef y As Double)
        Try
            alglib.pspline2calc(p.csobj, t, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3calc(ByVal p As pspline3interpolant, ByVal t As Double, ByRef x As Double, ByRef y As Double, ByRef z As Double)
        Try
            alglib.pspline3calc(p.csobj, t, x, y, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2tangent(ByVal p As pspline2interpolant, ByVal t As Double, ByRef x As Double, ByRef y As Double)
        Try
            alglib.pspline2tangent(p.csobj, t, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3tangent(ByVal p As pspline3interpolant, ByVal t As Double, ByRef x As Double, ByRef y As Double, ByRef z As Double)
        Try
            alglib.pspline3tangent(p.csobj, t, x, y, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2diff(ByVal p As pspline2interpolant, ByVal t As Double, ByRef x As Double, ByRef dx As Double, ByRef y As Double, ByRef dy As Double)
        Try
            alglib.pspline2diff(p.csobj, t, x, dx, y, dy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3diff(ByVal p As pspline3interpolant, ByVal t As Double, ByRef x As Double, ByRef dx As Double, ByRef y As Double, ByRef dy As Double, ByRef z As Double, ByRef dz As Double)
        Try
            alglib.pspline3diff(p.csobj, t, x, dx, y, dy, z, dz)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline2diff2(ByVal p As pspline2interpolant, ByVal t As Double, ByRef x As Double, ByRef dx As Double, ByRef d2x As Double, ByRef y As Double, ByRef dy As Double, ByRef d2y As Double)
        Try
            alglib.pspline2diff2(p.csobj, t, x, dx, d2x, y, dy, d2y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub pspline3diff2(ByVal p As pspline3interpolant, ByVal t As Double, ByRef x As Double, ByRef dx As Double, ByRef d2x As Double, ByRef y As Double, ByRef dy As Double, ByRef d2y As Double, ByRef z As Double, ByRef dz As Double, ByRef d2z As Double)
        Try
            alglib.pspline3diff2(p.csobj, t, x, dx, d2x, y, dy, d2y, z, dz, d2z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function pspline2arclength(ByVal p As pspline2interpolant, ByVal a As Double, ByVal b As Double) As Double
        Try
            pspline2arclength = alglib.pspline2arclength(p.csobj, a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function pspline3arclength(ByVal p As pspline3interpolant, ByVal a As Double, ByVal b As Double) As Double
        Try
            pspline3arclength = alglib.pspline3arclength(p.csobj, a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class spline2dinterpolant
        Public csobj As alglib.spline2dinterpolant
    End Class


    Public Sub spline2dbuildbilinear(ByVal x() As Double, ByVal y() As Double, ByVal f(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef c As spline2dinterpolant)
        Try
            c = New spline2dinterpolant()
            alglib.spline2dbuildbilinear(x, y, f, m, n, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dbuildbicubic(ByVal x() As Double, ByVal y() As Double, ByVal f(,) As Double, ByVal m As Integer, ByVal n As Integer, ByRef c As spline2dinterpolant)
        Try
            c = New spline2dinterpolant()
            alglib.spline2dbuildbicubic(x, y, f, m, n, c.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function spline2dcalc(ByVal c As spline2dinterpolant, ByVal x As Double, ByVal y As Double) As Double
        Try
            spline2dcalc = alglib.spline2dcalc(c.csobj, x, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub spline2ddiff(ByVal c As spline2dinterpolant, ByVal x As Double, ByVal y As Double, ByRef f As Double, ByRef fx As Double, ByRef fy As Double, ByRef fxy As Double)
        Try
            alglib.spline2ddiff(c.csobj, x, y, f, fx, fy, fxy)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dunpack(ByVal c As spline2dinterpolant, ByRef m As Integer, ByRef n As Integer, ByRef tbl(,) As Double)
        Try
            alglib.spline2dunpack(c.csobj, m, n, tbl)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dlintransxy(ByVal c As spline2dinterpolant, ByVal ax As Double, ByVal bx As Double, ByVal ay As Double, ByVal by As Double)
        Try
            alglib.spline2dlintransxy(c.csobj, ax, bx, ay, by)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dlintransf(ByVal c As spline2dinterpolant, ByVal a As Double, ByVal b As Double)
        Try
            alglib.spline2dlintransf(c.csobj, a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dresamplebicubic(ByVal a(,) As Double, ByVal oldheight As Integer, ByVal oldwidth As Integer, ByRef b(,) As Double, ByVal newheight As Integer, ByVal newwidth As Integer)
        Try
            alglib.spline2dresamplebicubic(a, oldheight, oldwidth, b, newheight, newwidth)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spline2dresamplebilinear(ByVal a(,) As Double, ByVal oldheight As Integer, ByVal oldwidth As Integer, ByRef b(,) As Double, ByVal newheight As Integer, ByVal newwidth As Integer)
        Try
            alglib.spline2dresamplebilinear(a, oldheight, oldwidth, b, newheight, newwidth)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function rmatrixludet(ByVal a(,) As Double, ByVal pivots() As Integer, ByVal n As Integer) As Double
        Try
            rmatrixludet = alglib.rmatrixludet(a, pivots, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixludet(ByVal a(,) As Double, ByVal pivots() As Integer) As Double
        Try
            rmatrixludet = alglib.rmatrixludet(a, pivots)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixdet(ByVal a(,) As Double, ByVal n As Integer) As Double
        Try
            rmatrixdet = alglib.rmatrixdet(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function rmatrixdet(ByVal a(,) As Double) As Double
        Try
            rmatrixdet = alglib.rmatrixdet(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixludet(ByVal a(,) As alglib.complex, ByVal pivots() As Integer, ByVal n As Integer) As alglib.complex
        Try
            cmatrixludet = alglib.cmatrixludet(a, pivots, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixludet(ByVal a(,) As alglib.complex, ByVal pivots() As Integer) As alglib.complex
        Try
            cmatrixludet = alglib.cmatrixludet(a, pivots)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixdet(ByVal a(,) As alglib.complex, ByVal n As Integer) As alglib.complex
        Try
            cmatrixdet = alglib.cmatrixdet(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function cmatrixdet(ByVal a(,) As alglib.complex) As alglib.complex
        Try
            cmatrixdet = alglib.cmatrixdet(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixcholeskydet(ByVal a(,) As Double, ByVal n As Integer) As Double
        Try
            spdmatrixcholeskydet = alglib.spdmatrixcholeskydet(a, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixcholeskydet(ByVal a(,) As Double) As Double
        Try
            spdmatrixcholeskydet = alglib.spdmatrixcholeskydet(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixdet(ByVal a(,) As Double, ByVal n As Integer, ByVal isupper As Boolean) As Double
        Try
            spdmatrixdet = alglib.spdmatrixdet(a, n, isupper)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function spdmatrixdet(ByVal a(,) As Double) As Double
        Try
            spdmatrixdet = alglib.spdmatrixdet(a)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function smatrixgevd(ByVal a(,) As Double, ByVal n As Integer, ByVal isuppera As Boolean, ByVal b(,) As Double, ByVal isupperb As Boolean, ByVal zneeded As Integer, ByVal problemtype As Integer, ByRef d() As Double, ByRef z(,) As Double) As Boolean
        Try
            smatrixgevd = alglib.smatrixgevd(a, n, isuppera, b, isupperb, zneeded, problemtype, d, z)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function smatrixgevdreduce(ByRef a(,) As Double, ByVal n As Integer, ByVal isuppera As Boolean, ByVal b(,) As Double, ByVal isupperb As Boolean, ByVal problemtype As Integer, ByRef r(,) As Double, ByRef isupperr As Boolean) As Boolean
        Try
            smatrixgevdreduce = alglib.smatrixgevdreduce(a, n, isuppera, b, isupperb, problemtype, r, isupperr)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub rmatrixinvupdatesimple(ByRef inva(,) As Double, ByVal n As Integer, ByVal updrow As Integer, ByVal updcolumn As Integer, ByVal updval As Double)
        Try
            alglib.rmatrixinvupdatesimple(inva, n, updrow, updcolumn, updval)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixinvupdaterow(ByRef inva(,) As Double, ByVal n As Integer, ByVal updrow As Integer, ByVal v() As Double)
        Try
            alglib.rmatrixinvupdaterow(inva, n, updrow, v)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixinvupdatecolumn(ByRef inva(,) As Double, ByVal n As Integer, ByVal updcolumn As Integer, ByVal u() As Double)
        Try
            alglib.rmatrixinvupdatecolumn(inva, n, updcolumn, u)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub rmatrixinvupdateuv(ByRef inva(,) As Double, ByVal n As Integer, ByVal u() As Double, ByVal v() As Double)
        Try
            alglib.rmatrixinvupdateuv(inva, n, u, v)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function rmatrixschur(ByRef a(,) As Double, ByVal n As Integer, ByRef s(,) As Double) As Boolean
        Try
            rmatrixschur = alglib.rmatrixschur(a, n, s)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function

    Public Class minasastate
        Public csobj As alglib.minasastate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class minasareport
        Public Property iterationscount() As Integer
        Get
            Return csobj.iterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.iterationscount = Value
        End Set
        End Property
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public Property activeconstraints() As Integer
        Get
            Return csobj.activeconstraints
        End Get
        Set(ByVal Value As Integer)
            csobj.activeconstraints = Value
        End Set
        End Property
        Public csobj As alglib.minasareport
    End Class


    Public Sub minasacreate(ByVal n As Integer, ByVal x() As Double, ByVal bndl() As Double, ByVal bndu() As Double, ByRef state As minasastate)
        Try
            state = New minasastate()
            alglib.minasacreate(n, x, bndl, bndu, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasacreate(ByVal x() As Double, ByVal bndl() As Double, ByVal bndu() As Double, ByRef state As minasastate)
        Try
            state = New minasastate()
            alglib.minasacreate(x, bndl, bndu, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasasetcond(ByVal state As minasastate, ByVal epsg As Double, ByVal epsf As Double, ByVal epsx As Double, ByVal maxits As Integer)
        Try
            alglib.minasasetcond(state.csobj, epsg, epsf, epsx, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasasetxrep(ByVal state As minasastate, ByVal needxrep As Boolean)
        Try
            alglib.minasasetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasasetalgorithm(ByVal state As minasastate, ByVal algotype As Integer)
        Try
            alglib.minasasetalgorithm(state.csobj, algotype)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasasetstpmax(ByVal state As minasastate, ByVal stpmax As Double)
        Try
            alglib.minasasetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function minasaiteration(ByVal state As minasastate) As Boolean
        Try
            minasaiteration = alglib.minasaiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear optimizer
    ' 
    ' These functions accept following parameters:
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' 
    '   -- ALGLIB --
    '      Copyright 20.03.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub minasaoptimize(ByVal state As minasastate, ByVal grad As ndimensional_grad, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minasa.minasastate = state.csobj.innerobj
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minasaoptimize()' (grad is null)")
        End If
        Try
            While alglib.minasa.minasaiteration(innerobj)
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minasaoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub minasaresults(ByVal state As minasastate, ByRef x() As Double, ByRef rep As minasareport)
        Try
            rep = New minasareport()
            alglib.minasaresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasaresultsbuf(ByVal state As minasastate, ByRef x() As Double, ByRef rep As minasareport)
        Try
            alglib.minasaresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minasarestartfrom(ByVal state As minasastate, ByVal x() As Double, ByVal bndl() As Double, ByVal bndu() As Double)
        Try
            alglib.minasarestartfrom(state.csobj, x, bndl, bndu)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class mincgstate
        Public csobj As alglib.mincgstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class mincgreport
        Public Property iterationscount() As Integer
        Get
            Return csobj.iterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.iterationscount = Value
        End Set
        End Property
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public csobj As alglib.mincgreport
    End Class


    Public Sub mincgcreate(ByVal n As Integer, ByVal x() As Double, ByRef state As mincgstate)
        Try
            state = New mincgstate()
            alglib.mincgcreate(n, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgcreate(ByVal x() As Double, ByRef state As mincgstate)
        Try
            state = New mincgstate()
            alglib.mincgcreate(x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgsetcond(ByVal state As mincgstate, ByVal epsg As Double, ByVal epsf As Double, ByVal epsx As Double, ByVal maxits As Integer)
        Try
            alglib.mincgsetcond(state.csobj, epsg, epsf, epsx, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgsetxrep(ByVal state As mincgstate, ByVal needxrep As Boolean)
        Try
            alglib.mincgsetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgsetcgtype(ByVal state As mincgstate, ByVal cgtype As Integer)
        Try
            alglib.mincgsetcgtype(state.csobj, cgtype)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgsetstpmax(ByVal state As mincgstate, ByVal stpmax As Double)
        Try
            alglib.mincgsetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgsuggeststep(ByVal state As mincgstate, ByVal stp As Double)
        Try
            alglib.mincgsuggeststep(state.csobj, stp)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function mincgiteration(ByVal state As mincgstate) As Boolean
        Try
            mincgiteration = alglib.mincgiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear optimizer
    ' 
    ' These functions accept following parameters:
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' 
    '   -- ALGLIB --
    '      Copyright 20.04.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub mincgoptimize(ByVal state As mincgstate, ByVal grad As ndimensional_grad, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.mincg.mincgstate = state.csobj.innerobj
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'mincgoptimize()' (grad is null)")
        End If
        Try
            While alglib.mincg.mincgiteration(innerobj)
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'mincgoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub mincgresults(ByVal state As mincgstate, ByRef x() As Double, ByRef rep As mincgreport)
        Try
            rep = New mincgreport()
            alglib.mincgresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgresultsbuf(ByVal state As mincgstate, ByRef x() As Double, ByRef rep As mincgreport)
        Try
            alglib.mincgresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub mincgrestartfrom(ByVal state As mincgstate, ByVal x() As Double)
        Try
            alglib.mincgrestartfrom(state.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class minbleicstate
        Public csobj As alglib.minbleicstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    'This structure stores optimization report:
    '* InnerIterationsCount      number of inner iterations
    '* OuterIterationsCount      number of outer iterations
    '* NFEV                      number of gradient evaluations
    '
    'There are additional fields which can be used for debugging:
    '* DebugEqErr                error in the equality constraints (2-norm)
    '* DebugFS                   f, calculated at projection of initial point
    '                            to the feasible set
    '* DebugFF                   f, calculated at the final point
    '* DebugDX                   |X_start-X_final|
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class minbleicreport
        Public Property inneriterationscount() As Integer
        Get
            Return csobj.inneriterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.inneriterationscount = Value
        End Set
        End Property
        Public Property outeriterationscount() As Integer
        Get
            Return csobj.outeriterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.outeriterationscount = Value
        End Set
        End Property
        Public Property nfev() As Integer
        Get
            Return csobj.nfev
        End Get
        Set(ByVal Value As Integer)
            csobj.nfev = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public Property debugeqerr() As Double
        Get
            Return csobj.debugeqerr
        End Get
        Set(ByVal Value As Double)
            csobj.debugeqerr = Value
        End Set
        End Property
        Public Property debugfs() As Double
        Get
            Return csobj.debugfs
        End Get
        Set(ByVal Value As Double)
            csobj.debugfs = Value
        End Set
        End Property
        Public Property debugff() As Double
        Get
            Return csobj.debugff
        End Get
        Set(ByVal Value As Double)
            csobj.debugff = Value
        End Set
        End Property
        Public Property debugdx() As Double
        Get
            Return csobj.debugdx
        End Get
        Set(ByVal Value As Double)
            csobj.debugdx = Value
        End Set
        End Property
        Public csobj As alglib.minbleicreport
    End Class


    Public Sub minbleiccreate(ByVal n As Integer, ByVal x() As Double, ByRef state As minbleicstate)
        Try
            state = New minbleicstate()
            alglib.minbleiccreate(n, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleiccreate(ByVal x() As Double, ByRef state As minbleicstate)
        Try
            state = New minbleicstate()
            alglib.minbleiccreate(x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetbc(ByVal state As minbleicstate, ByVal bndl() As Double, ByVal bndu() As Double)
        Try
            alglib.minbleicsetbc(state.csobj, bndl, bndu)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetlc(ByVal state As minbleicstate, ByVal c(,) As Double, ByVal ct() As Integer, ByVal k As Integer)
        Try
            alglib.minbleicsetlc(state.csobj, c, ct, k)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetlc(ByVal state As minbleicstate, ByVal c(,) As Double, ByVal ct() As Integer)
        Try
            alglib.minbleicsetlc(state.csobj, c, ct)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetinnercond(ByVal state As minbleicstate, ByVal epsg As Double, ByVal epsf As Double, ByVal epsx As Double)
        Try
            alglib.minbleicsetinnercond(state.csobj, epsg, epsf, epsx)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetoutercond(ByVal state As minbleicstate, ByVal epsx As Double, ByVal epsi As Double)
        Try
            alglib.minbleicsetoutercond(state.csobj, epsx, epsi)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetbarrierwidth(ByVal state As minbleicstate, ByVal mu As Double)
        Try
            alglib.minbleicsetbarrierwidth(state.csobj, mu)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetbarrierdecay(ByVal state As minbleicstate, ByVal mudecay As Double)
        Try
            alglib.minbleicsetbarrierdecay(state.csobj, mudecay)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetmaxits(ByVal state As minbleicstate, ByVal maxits As Integer)
        Try
            alglib.minbleicsetmaxits(state.csobj, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetxrep(ByVal state As minbleicstate, ByVal needxrep As Boolean)
        Try
            alglib.minbleicsetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicsetstpmax(ByVal state As minbleicstate, ByVal stpmax As Double)
        Try
            alglib.minbleicsetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function minbleiciteration(ByVal state As minbleicstate) As Boolean
        Try
            minbleiciteration = alglib.minbleiciteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear optimizer
    ' 
    ' These functions accept following parameters:
    '     grad    -   callback which calculates function (or merit function)
    '                 value func and gradient grad at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' 
    '   -- ALGLIB --
    '      Copyright 28.11.2010 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub minbleicoptimize(ByVal state As minbleicstate, ByVal grad As ndimensional_grad, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.minbleic.minbleicstate = state.csobj.innerobj
        If grad Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'minbleicoptimize()' (grad is null)")
        End If
        Try
            While alglib.minbleic.minbleiciteration(innerobj)
                If innerobj.needfg Then
                    grad(innerobj.x, innerobj.f, innerobj.g, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'minbleicoptimize' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub minbleicresults(ByVal state As minbleicstate, ByRef x() As Double, ByRef rep As minbleicreport)
        Try
            rep = New minbleicreport()
            alglib.minbleicresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicresultsbuf(ByVal state As minbleicstate, ByRef x() As Double, ByRef rep As minbleicreport)
        Try
            alglib.minbleicresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub minbleicrestartfrom(ByVal state As minbleicstate, ByVal x() As Double)
        Try
            alglib.minbleicrestartfrom(state.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub

    Public Class nleqstate
        Public csobj As alglib.nleqstate
    End Class
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class nleqreport
        Public Property iterationscount() As Integer
        Get
            Return csobj.iterationscount
        End Get
        Set(ByVal Value As Integer)
            csobj.iterationscount = Value
        End Set
        End Property
        Public Property nfunc() As Integer
        Get
            Return csobj.nfunc
        End Get
        Set(ByVal Value As Integer)
            csobj.nfunc = Value
        End Set
        End Property
        Public Property njac() As Integer
        Get
            Return csobj.njac
        End Get
        Set(ByVal Value As Integer)
            csobj.njac = Value
        End Set
        End Property
        Public Property terminationtype() As Integer
        Get
            Return csobj.terminationtype
        End Get
        Set(ByVal Value As Integer)
            csobj.terminationtype = Value
        End Set
        End Property
        Public csobj As alglib.nleqreport
    End Class


    Public Sub nleqcreatelm(ByVal n As Integer, ByVal m As Integer, ByVal x() As Double, ByRef state As nleqstate)
        Try
            state = New nleqstate()
            alglib.nleqcreatelm(n, m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqcreatelm(ByVal m As Integer, ByVal x() As Double, ByRef state As nleqstate)
        Try
            state = New nleqstate()
            alglib.nleqcreatelm(m, x, state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqsetcond(ByVal state As nleqstate, ByVal epsf As Double, ByVal maxits As Integer)
        Try
            alglib.nleqsetcond(state.csobj, epsf, maxits)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqsetxrep(ByVal state As nleqstate, ByVal needxrep As Boolean)
        Try
            alglib.nleqsetxrep(state.csobj, needxrep)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqsetstpmax(ByVal state As nleqstate, ByVal stpmax As Double)
        Try
            alglib.nleqsetstpmax(state.csobj, stpmax)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Function nleqiteration(ByVal state As nleqstate) As Boolean
        Try
            nleqiteration = alglib.nleqiteration(state.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' This family of functions is used to launcn iterations of nonlinear solver
    ' 
    ' These functions accept following parameters:
    '     func    -   callback which calculates function (or merit function)
    '                 value func at given point x
    '     jac     -   callback which calculates function vector fi[]
    '                 and Jacobian jac at given point x
    '     rep     -   optional callback which is called after each iteration
    '                 can be null
    '     obj     -   optional object which is passed to func/grad/hess/jac/rep
    '                 can be null
    ' 
    ' 
    ' 
    '   -- ALGLIB --
    '      Copyright 20.03.2009 by Bochkanov Sergey
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Sub nleqsolve(ByVal state As nleqstate, ByVal func As ndimensional_func, ByVal jac As ndimensional_jac, ByVal rep As ndimensional_rep, ByVal obj As Object)
        Dim innerobj As alglib.nleq.nleqstate = state.csobj.innerobj
        If func Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'nleqsolve()' (func is null)")
        End If
        If jac Is Nothing Then
            Throw New AlglibException("ALGLIB: error in 'nleqsolve()' (jac is null)")
        End If
        Try
            While alglib.nleq.nleqiteration(innerobj)
                If innerobj.needf Then
                    func(innerobj.x,  innerobj.f, obj)
                    Continue While
                End If
                If innerobj.needfij Then
                    jac(innerobj.x, innerobj.fi, innerobj.j, obj)
                    Continue While
                End If
                If innerobj.xupdated Then
                    If rep Isnot Nothing Then
                        rep(innerobj.x, innerobj.f, obj)
                    End If
                    Continue While
                End If
                Throw New AlglibException("ALGLIB: error in 'nleqsolve' (some derivatives were not provided?)")
            End While
        Catch E As alglib.alglibexception
            Throw New AlglibException(E.Msg)
        End Try
    End Sub




    Public Sub nleqresults(ByVal state As nleqstate, ByRef x() As Double, ByRef rep As nleqreport)
        Try
            rep = New nleqreport()
            alglib.nleqresults(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqresultsbuf(ByVal state As nleqstate, ByRef x() As Double, ByRef rep As nleqreport)
        Try
            alglib.nleqresultsbuf(state.csobj, x, rep.csobj)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub nleqrestartfrom(ByVal state As nleqstate, ByVal x() As Double)
        Try
            alglib.nleqrestartfrom(state.csobj, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub airy(ByVal x As Double, ByRef ai As Double, ByRef aip As Double, ByRef bi As Double, ByRef bip As Double)
        Try
            alglib.airy(x, ai, aip, bi, bip)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function besselj0(ByVal x As Double) As Double
        Try
            besselj0 = alglib.besselj0(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besselj1(ByVal x As Double) As Double
        Try
            besselj1 = alglib.besselj1(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besseljn(ByVal n As Integer, ByVal x As Double) As Double
        Try
            besseljn = alglib.besseljn(n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function bessely0(ByVal x As Double) As Double
        Try
            bessely0 = alglib.bessely0(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function bessely1(ByVal x As Double) As Double
        Try
            bessely1 = alglib.bessely1(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besselyn(ByVal n As Integer, ByVal x As Double) As Double
        Try
            besselyn = alglib.besselyn(n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besseli0(ByVal x As Double) As Double
        Try
            besseli0 = alglib.besseli0(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besseli1(ByVal x As Double) As Double
        Try
            besseli1 = alglib.besseli1(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besselk0(ByVal x As Double) As Double
        Try
            besselk0 = alglib.besselk0(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besselk1(ByVal x As Double) As Double
        Try
            besselk1 = alglib.besselk1(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function besselkn(ByVal nn As Integer, ByVal x As Double) As Double
        Try
            besselkn = alglib.besselkn(nn, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function beta(ByVal a As Double, ByVal b As Double) As Double
        Try
            beta = alglib.beta(a, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function incompletebeta(ByVal a As Double, ByVal b As Double, ByVal x As Double) As Double
        Try
            incompletebeta = alglib.incompletebeta(a, b, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invincompletebeta(ByVal a As Double, ByVal b As Double, ByVal y As Double) As Double
        Try
            invincompletebeta = alglib.invincompletebeta(a, b, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function binomialdistribution(ByVal k As Integer, ByVal n As Integer, ByVal p As Double) As Double
        Try
            binomialdistribution = alglib.binomialdistribution(k, n, p)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function binomialcdistribution(ByVal k As Integer, ByVal n As Integer, ByVal p As Double) As Double
        Try
            binomialcdistribution = alglib.binomialcdistribution(k, n, p)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invbinomialdistribution(ByVal k As Integer, ByVal n As Integer, ByVal y As Double) As Double
        Try
            invbinomialdistribution = alglib.invbinomialdistribution(k, n, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function chebyshevcalculate(ByVal r As Integer, ByVal n As Integer, ByVal x As Double) As Double
        Try
            chebyshevcalculate = alglib.chebyshevcalculate(r, n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function chebyshevsum(ByVal c() As Double, ByVal r As Integer, ByVal n As Integer, ByVal x As Double) As Double
        Try
            chebyshevsum = alglib.chebyshevsum(c, r, n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub chebyshevcoefficients(ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.chebyshevcoefficients(n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub fromchebyshev(ByVal a() As Double, ByVal n As Integer, ByRef b() As Double)
        Try
            alglib.fromchebyshev(a, n, b)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function chisquaredistribution(ByVal v As Double, ByVal x As Double) As Double
        Try
            chisquaredistribution = alglib.chisquaredistribution(v, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function chisquarecdistribution(ByVal v As Double, ByVal x As Double) As Double
        Try
            chisquarecdistribution = alglib.chisquarecdistribution(v, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invchisquaredistribution(ByVal v As Double, ByVal y As Double) As Double
        Try
            invchisquaredistribution = alglib.invchisquaredistribution(v, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function dawsonintegral(ByVal x As Double) As Double
        Try
            dawsonintegral = alglib.dawsonintegral(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function ellipticintegralk(ByVal m As Double) As Double
        Try
            ellipticintegralk = alglib.ellipticintegralk(m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function ellipticintegralkhighprecision(ByVal m1 As Double) As Double
        Try
            ellipticintegralkhighprecision = alglib.ellipticintegralkhighprecision(m1)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function incompleteellipticintegralk(ByVal phi As Double, ByVal m As Double) As Double
        Try
            incompleteellipticintegralk = alglib.incompleteellipticintegralk(phi, m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function ellipticintegrale(ByVal m As Double) As Double
        Try
            ellipticintegrale = alglib.ellipticintegrale(m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function incompleteellipticintegrale(ByVal phi As Double, ByVal m As Double) As Double
        Try
            incompleteellipticintegrale = alglib.incompleteellipticintegrale(phi, m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function exponentialintegralei(ByVal x As Double) As Double
        Try
            exponentialintegralei = alglib.exponentialintegralei(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function exponentialintegralen(ByVal x As Double, ByVal n As Integer) As Double
        Try
            exponentialintegralen = alglib.exponentialintegralen(x, n)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function fdistribution(ByVal a As Integer, ByVal b As Integer, ByVal x As Double) As Double
        Try
            fdistribution = alglib.fdistribution(a, b, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function fcdistribution(ByVal a As Integer, ByVal b As Integer, ByVal x As Double) As Double
        Try
            fcdistribution = alglib.fcdistribution(a, b, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invfdistribution(ByVal a As Integer, ByVal b As Integer, ByVal y As Double) As Double
        Try
            invfdistribution = alglib.invfdistribution(a, b, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub fresnelintegral(ByVal x As Double, ByRef c As Double, ByRef s As Double)
        Try
            alglib.fresnelintegral(x, c, s)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function hermitecalculate(ByVal n As Integer, ByVal x As Double) As Double
        Try
            hermitecalculate = alglib.hermitecalculate(n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function hermitesum(ByVal c() As Double, ByVal n As Integer, ByVal x As Double) As Double
        Try
            hermitesum = alglib.hermitesum(c, n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub hermitecoefficients(ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.hermitecoefficients(n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub jacobianellipticfunctions(ByVal u As Double, ByVal m As Double, ByRef sn As Double, ByRef cn As Double, ByRef dn As Double, ByRef ph As Double)
        Try
            alglib.jacobianellipticfunctions(u, m, sn, cn, dn, ph)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function laguerrecalculate(ByVal n As Integer, ByVal x As Double) As Double
        Try
            laguerrecalculate = alglib.laguerrecalculate(n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function laguerresum(ByVal c() As Double, ByVal n As Integer, ByVal x As Double) As Double
        Try
            laguerresum = alglib.laguerresum(c, n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub laguerrecoefficients(ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.laguerrecoefficients(n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function legendrecalculate(ByVal n As Integer, ByVal x As Double) As Double
        Try
            legendrecalculate = alglib.legendrecalculate(n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function legendresum(ByVal c() As Double, ByVal n As Integer, ByVal x As Double) As Double
        Try
            legendresum = alglib.legendresum(c, n, x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Sub legendrecoefficients(ByVal n As Integer, ByRef c() As Double)
        Try
            alglib.legendrecoefficients(n, c)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Function poissondistribution(ByVal k As Integer, ByVal m As Double) As Double
        Try
            poissondistribution = alglib.poissondistribution(k, m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function poissoncdistribution(ByVal k As Integer, ByVal m As Double) As Double
        Try
            poissoncdistribution = alglib.poissoncdistribution(k, m)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invpoissondistribution(ByVal k As Integer, ByVal y As Double) As Double
        Try
            invpoissondistribution = alglib.invpoissondistribution(k, y)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function psi(ByVal x As Double) As Double
        Try
            psi = alglib.psi(x)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Function studenttdistribution(ByVal k As Integer, ByVal t As Double) As Double
        Try
            studenttdistribution = alglib.studenttdistribution(k, t)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function


    Public Function invstudenttdistribution(ByVal k As Integer, ByVal p As Double) As Double
        Try
            invstudenttdistribution = alglib.invstudenttdistribution(k, p)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Function




    Public Sub sinecosineintegrals(ByVal x As Double, ByRef si As Double, ByRef ci As Double)
        Try
            alglib.sinecosineintegrals(x, si, ci)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub hyperbolicsinecosineintegrals(ByVal x As Double, ByRef shi As Double, ByRef chi As Double)
        Try
            alglib.hyperbolicsinecosineintegrals(x, shi, chi)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub pearsoncorrelationsignificance(ByVal r As Double, ByVal n As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.pearsoncorrelationsignificance(r, n, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub spearmanrankcorrelationsignificance(ByVal r As Double, ByVal n As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.spearmanrankcorrelationsignificance(r, n, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub jarqueberatest(ByVal x() As Double, ByVal n As Integer, ByRef p As Double)
        Try
            alglib.jarqueberatest(x, n, p)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub mannwhitneyutest(ByVal x() As Double, ByVal n As Integer, ByVal y() As Double, ByVal m As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.mannwhitneyutest(x, n, y, m, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub onesamplesigntest(ByVal x() As Double, ByVal n As Integer, ByVal median As Double, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.onesamplesigntest(x, n, median, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub studentttest1(ByVal x() As Double, ByVal n As Integer, ByVal mean As Double, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.studentttest1(x, n, mean, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub studentttest2(ByVal x() As Double, ByVal n As Integer, ByVal y() As Double, ByVal m As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.studentttest2(x, n, y, m, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub unequalvariancettest(ByVal x() As Double, ByVal n As Integer, ByVal y() As Double, ByVal m As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.unequalvariancettest(x, n, y, m, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub ftest(ByVal x() As Double, ByVal n As Integer, ByVal y() As Double, ByVal m As Integer, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.ftest(x, n, y, m, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub


    Public Sub onesamplevariancetest(ByVal x() As Double, ByVal n As Integer, ByVal variance As Double, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.onesamplevariancetest(x, n, variance, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




    Public Sub wilcoxonsignedranktest(ByVal x() As Double, ByVal n As Integer, ByVal e As Double, ByRef bothtails As Double, ByRef lefttail As Double, ByRef righttail As Double)
        Try
            alglib.wilcoxonsignedranktest(x, n, e, bothtails, lefttail, righttail)
        Catch _E_Alglib As alglib.alglibexception
            Throw New AlglibException(_E_Alglib.Msg)
        End Try
    End Sub




End Module
