Imports System.Math
Public Class ClsPrayers
    Const PI As Double = 3.141592654
    Const AB2 As Double = -0.833333333
    Dim Dohr, Asr1, Asr2, Magrib, Esha, Fajer, Shroq As String
    Dim mvarDay As Double
    Dim mvarMonth As Double
    Dim mvarYear As Double
    Dim mvarZone As Integer
    Dim mvarLongitude As Double
    Dim mvarLatitude As Double
    Dim mvarArcEsha As Double
    Dim mvarArcFajer As Double
    Dim M As Double


    Private Sub GetPT()
        Dim D As Double
        Dim L As Double
        Dim M As Double
        Dim Lambda As Double
        Dim ST As Double
        Dim Noon As Double
        Dim UTNoon As Double
        Dim LocalNoon As Double
        Dim AsrAlt1 As Double
        Dim AsrAlt2 As Double
        Dim AsrArc1 As Double
        Dim AsrArc2 As Double
        Dim DurinalArc As Double
        Dim SunRise As Double
        Dim SunSet As Double
        Dim EshaArc As Double
        Dim FajrArc As Double
        Dim Obliquity As Double
        Dim Alpha As Double
        Dim Dec As Double
        ' اليوم الجولياني
        D = ((367 * mvarYear) - (Int((7 / 4) * (mvarYear + Int((mvarMonth + 9) / 12)))) + Int(275 * (mvarMonth / 9)) + mvarDay - 730531.5)
        ' طول الشمس الوسطى
        L = Fix360((280.461 + 0.9856474 * D))
        'حصة الشمس الوسطى
        M = Fix360(357.528 + 0.9856003 * D)
        'طول الشمس البروجى
        Lambda = Fix360(L + 1.915 * Sin(M * PI / 180) + 0.02 * Sin(2 * M * PI / 180))
        'ميل دائرة البروج
        Obliquity = 23.439 - 0.0000004 * D
        'المطلع المستقيم
        Alpha = Atan(Cos(Obliquity * PI / 180) * Tan(Lambda * PI / 180)) * 180 / PI
        If (Alpha > 90) And (Alpha < 180) Then Alpha = Alpha + 180
        If (Alpha > 180) And (Alpha < 360) Then Alpha = Alpha + 360
        'نعدل المطلع المستقيم
        Alpha = Fix360(Alpha + 90 * (Int(Lambda / 90) - Int(Alpha / 90)))
        ' الزمن النجمى
        ST = Fix360(100.46 + 0.985647352 * D)
        Dec = aSin(Sin(Obliquity * PI / 180) * Sin(Lambda * PI / 180)) * 180 / PI
        'زوال الشمس الوسطى
        Noon = Alpha - ST
        If Noon < 0 Then Noon = Noon + 360
        'الزوالى العالمى
        UTNoon = Noon - mvarLongitude
        'الزوال المحلى
        LocalNoon = UTNoon / 15 + mvarZone
        Dohr = CStr(Num2Time(CDbl(LocalNoon)))  'الظهر
        'وقت صلاة العصر الأول وهو المذهب الشافعى " وهو معمول به فى كثير من الأقطار
        AsrAlt1 = Atan(1 + Tan((mvarLatitude - Dec) * PI / 180)) * 180 / PI
        'صلاة العصر الثانى " المذهب الحنفى
        AsrAlt2 = Atan(2 + Tan((mvarLatitude - Dec) * PI / 180)) * 180 / PI
        ' قوس العصر الشافعى
        AsrArc1 = aCos((Sin((90 - AsrAlt1) * PI / 180) - Sin(Dec * PI / 180) * Sin(mvarLatitude * PI / 180)) / (Cos(Dec * PI / 180) * Cos(mvarLatitude * PI / 180))) * 180 / PI / 15
        ' قوس العصر الحنفى
        AsrArc2 = aCos((Sin((90 - AsrAlt2) * PI / 180) - Sin(Dec * PI / 180) * Sin(mvarLatitude * PI / 180)) / (Cos(Dec * PI / 180) * Cos(mvarLatitude * PI / 180))) * 180 / PI / 15
        Asr1 = CStr(Num2Time(LocalNoon + AsrArc1))  'العصر الشافعي
        Asr2 = CStr(Num2Time(LocalNoon + AsrArc2))  'العصر الحنفى
        'نصف قوس النهار
        DurinalArc = aCos((Sin(AB2 * PI / 180) - Sin(Dec * PI / 180) * Sin(mvarLatitude * PI / 180)) / (Cos(Dec * PI / 180) * Cos(mvarLatitude * PI / 180))) * 180 / PI
        'وقت الشروق
        SunRise = LocalNoon - (DurinalArc / 15)
        Shroq = CStr(Num2Time(CDbl(SunRise))) 'الشروق
        'وقت الغروب
        SunSet = LocalNoon + (DurinalArc / 15)
        Magrib = CStr(Num2Time(CDbl(SunSet))) 'المغرب
        'فضل دائرة العشاء
        EshaArc = aCos((Sin(mvarArcEsha * PI / 180) - Sin(Dec * PI / 180) * Sin(mvarLatitude * PI / 180)) / (Cos(Dec * PI / 180) * Cos(mvarLatitude * PI / 180))) * 180 / PI
        Esha = CStr(Num2Time(LocalNoon + (EshaArc / 15))) 'العشاء
        ' فضل دائر الفجر
        FajrArc = aCos((Sin(mvarArcFajer * PI / 180) - Sin(Dec * PI / 180) * Sin(mvarLatitude * PI / 180)) / (Cos(Dec * PI / 180) * Cos(mvarLatitude * PI / 180))) * 180 / PI
        Fajer = CStr(Num2Time(LocalNoon - (FajrArc / 15))) 'الفجر

    End Sub
    Private Function Fix360(x As Double) As Double
        If x > 360 Then Fix360 = ((x / 360) - Int(x / 360)) * 360 Else Fix360 = x
    End Function
    Private Function Num2Time(x As Double) As String
        Dim h As Double
        Dim B As Double
        Dim MM As Double
        Dim S As Double
        Dim FullTime As String
        Dim ShortTime As Date
        h = (Int(x))
        B = (x - h)
        MM = (B * 60)
        M = Int(MM)
        B =  (MM - M)
        ' s = Int(B * 60)
        S = 0
        FullTime = CStr(Trim(CStr(h)) & ":" & Trim(CStr(M)) & ":" & Trim(CStr(S)))
        If S >= 30 Then S = 0 : M = M + 1 : S = 0
        If M > 59 Then M = 0 : h = h + 1
        ShortTime = CDate(Trim(CStr(h)) & ":" & Trim(CStr(M)))
        Num2Time = CStr(ShortTime)
    End Function
    Private Function aSin(x As Double) As Double
        Select Case x
            Case 1 : aSin = PI / 2
            Case -1 : aSin = (3 * PI) / 2
            Case Else : aSin = Atan(x / Sqrt(-x * x + 1))
        End Select
    End Function
    Private Function aCos(x As Double) As Double
        aCos = Atan(-x / Sqrt(-x * x + 1)) + 2 * Atan(1)
    End Function
    Public Property LocalDay() As Double
        'LocalDay = mvarDay
        Get
            Return mvarDay

        End Get
        Set(value As Double)
            mvarDay = CDbl(value)
        End Set
    End Property
    Public Property LocalMonth() As Double
        'LocalDay = mvarDay
        Get
            Return mvarMonth

        End Get
        Set(value As Double)
            mvarMonth = CDbl(value)
        End Set
    End Property

    Public Property LocalYear() As Double
        'LocalDay = mvarDay
        Get
            Return mvarYear

        End Get
        Set(value As Double)
            mvarYear = CDbl(value)
        End Set
    End Property


    Public Property Zone() As Integer
        'LocalDay = mvarDay
        Get
            Return mvarZone

        End Get
        Set(value As Integer)
            mvarZone = CInt(value)
        End Set
    End Property



    Public Property Longitude() As Double
        'LocalDay = mvarDay
        Get
            Return mvarLongitude

        End Get
        Set(value As Double)
            mvarLongitude = CDbl(value)
        End Set
    End Property

    Public Property Latitude() As Double
        'LocalDay = mvarDay
        Get
            Return mvarLatitude

        End Get
        Set(value As Double)
            mvarLatitude = CDbl(value)
        End Set
    End Property

    Public Property ArcEsha() As Double
        'LocalDay = mvarDay
        Get
            Return CSng(mvarLatitude)

        End Get
        Set(value As Double)
            mvarArcEsha = CSng(value)
        End Set
    End Property
    Public Property ArcFajer() As Double
        'LocalDay = mvarDay
        Get
            Return mvarArcFajer

        End Get
        Set(value As Double)
            mvarArcFajer = CSng(value)
        End Set
    End Property
    Public ReadOnly Property Time_Fajer() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Fajer

        End Get
    End Property
    Public ReadOnly Property Time_Dohr() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Dohr

        End Get
    End Property
    Public ReadOnly Property Time_Asr_Shafee() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Asr1

        End Get
    End Property
    Public ReadOnly Property Time_Asr_Hanafee() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Asr2

        End Get
    End Property
    Public ReadOnly Property Time_Magrib() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Magrib

        End Get
    End Property
    Public ReadOnly Property Time_Esha() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Esha

        End Get
    End Property
    Public ReadOnly Property Time_Shroq() As String
        'LocalDay = mvarDay
        Get
            GetPT()
            Return Shroq

        End Get
    End Property
End Class
