drop database TecPruebas
Create database TecPruebas

Use TecPruebas 

Create table Usuarios(
NoEmpleado Nvarchar(255) primary key not null,
Nombre NVarchar(255) not null,
ApellidoPaterno NVarchar(255) not null,
ApellidoMaterno NVarchar(255) not null,
TipoUsuario NVarchar(20) NOT NULL CHECK (TipoUsuario IN ('Docente', 'Administrativo', 'SubDirector', 'JefeDP/Div'))
)


insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040411HL','Armando','Zambrano','Juarez','Docente')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040411HS','Armando','Zambrano','Juarez','Administrativo')

select *from Usuarios
Select NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario from Usuarios where NoEmpleado = 'ZAJA01040411HL'

CATALOGOAREAS DE ADSCRIPCION
                    SELECT TipoUsuario FROM Usuarios WHERE NoEmpleado = 'ZAJA01040411HL'

					select PEM.Solicitudes, PEM.Fecha, concat(Us.Nombre, ' ', Us.ApellidoPaterno, ' ', Us.ApellidoMaterno) as Usuario, PEM.Estado From PERMISO PEM inner join Usuarios Us on Us.NoEmpleado = PEM.NoEmpPro WHERE Us.NoEmpleado ='ZAJA01040411HL'


Create table PERMISO(
Solicitudes int primary key not null identity(1,1), 
Fecha datetime  null,	
NoEmpPro nVarchar(255) not null,
TipoUsuario Nvarchar(20),
dias int not null,
DeLAfECHA datetime not null,
aLAFECHA datetime not null,
horainicio time  null,
horafin time  null,
AREAADSCRIPCION Nvarchar(20),
HFrenGrupo decimal(4,2) null,
HApoCad decimal(4,2) null,
MOTIVO Nvarchar(100) not null,
Goce bit  null,
sinGoce bit  null,
OBSERVACIONES Nvarchar(100) null,
Estado varchar(14) not null
CONSTRAINT Estado CHECK (Estado IN ('Espera', 'Aceptado', 'Rechazado')),
QUIENAUTORIZA Nvarchar(100)
foreign key (NoEmpPro) references Usuarios(NoEmpleado)
)

 select PM.Solicitudes, CONCAT(US.Nombre, US.ApellidoPaterno, US.ApellidoMaterno)as Nombre, PM.NoEmpPro, PM.TipoUsuario,
  PM.DeLAfECHA, PM.DeLAfECHA From PERMISO PM inner join Usuarios US on US.NoEmpleado = PM.NoEmpPro
select *From PERMISO


select CONCAT(Us.Nombre, ' ' , Us.ApellidoPaterno, ' ', Us.ApellidoMaterno) AS Nombre, PM.NoEmpPro, PM.TipoUsuario, PM.dias,
PM.DeLAfECHA, PM.aLAFECHA, PM.horainicio, PM.horafin, PM.MOTIVO  from PERMISO PM inner join Usuarios Us on Us.NoEmpleado =  PM.NoEmpPro where  PM.Solicitudes=2


select *from PERMISO
Select PEM.Solicitudes, PEM.Fecha, concat(Us.Nombre, ' ', Us.ApellidoPaterno, ' ', Us.ApellidoMaterno) as Usuario From PERMISO PEM inner join Usuarios Us on Us.NoEmpleado = PEM.NoEmpPro  where Estado = 'aceptado'


UPDATE PERMISO
SET Goce = 1, 
    sinGoce = 0, 
    OBSERVACIONES = 'Casa', 
    QUIENAUTORIZA = 'Pablo',
	Estado = 'Aceptado'
WHERE Solicitudes = 1;


Select PEM.Solicitudes, PEM.Fecha, concat(Us.Nombre, ' ', Us.ApellidoPaterno, ' ', Us.ApellidoMaterno) as Usuario, PEM.Estado" +
                                      "From PERMISO PEM inner join Usuarios Us on Us.NoEmpleado = PEM.NoEmpPro WHERE Us.NoEmpleado = 'ZAJA01040411HS'


SELECT *FROM PERMISO