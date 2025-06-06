drop database TecPruebas
Create database TecPruebas

Use TecPruebas 

EXEC sp_configure 'remote access', 1;
RECONFIGURE;


insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('2025010070','Angel','Sanchez','Jimenez','Docente')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('SAJA020118HSLNMNA8S','Angel','Sanchez','Jimenez','Administrativo')

select *from Usuarios
Select NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario from Usuarios where NoEmpleado = 'SAJA020118HSLNMNA8'

--CATALOGOAREAS DE ADSCRIPCION
                    SELECT TipoUsuario FROM Usuarios WHERE NoEmpleado = 'SAJA020118HSLNMNA8'

					select PEM.Solicitudes, PEM.Fecha, concat(Us.Nombre, ' ', Us.ApellidoPaterno, ' ', Us.ApellidoMaterno) as Usuario, PEM.Estado From PERMISO PEM inner join Usuarios Us on Us.NoEmpleado = PEM.NoEmpPro WHERE Us.NoEmpleado ='ZAJA01040411HL'

SELECT COUNT(*) FROM Usuarios WHERE NoEmpleado = 'SAJA020118HSLNMNA8'

Create table Usuarios(
NoEmpleado Nvarchar(255) primary key not null,
Nombre NVarchar(255) not null,
ApellidoPaterno NVarchar(255) not null,
ApellidoMaterno NVarchar(255) not null,
TipoUsuario NVarchar(20) NOT NULL CHECK (TipoUsuario IN ('Docente', 'Administrativo', 'SubDirector', 'JefeDP/Div', 'Director'))
)

Create table PERMISO(
Solicitudes int primary key not null identity(1,1), 
Fecha datetime  null,	
NoEmpPro nVarchar(255) not null,
TipoUsuario Nvarchar(20),
dias int null,
pdf VARBINARY(MAX) null,
actividad nvarchar(200) null,
DeLAfECHA datetime  null,
aLAFECHA datetime  null,
horainicio time  null,
horafin time  null,
AREAADSCRIPCION Nvarchar(60),
HFrenGrupo decimal(4,2) null,
HApoCad decimal(4,2) null,
MOTIVO Nvarchar(100)  null,
tipoMotivo Nvarchar(20) not null
constraint tipoMotivo Check(tipoMotivo IN('Personal','Salud')),
categorias Nvarchar(50) null
constraint categorias check(categorias In('Maternidad', 'Paternidad', 'Fallecimiento', 'Baja Medica', 'Urgencia')),
Goce bit  null,
sinGoce bit  null,
OBSERVACIONES Nvarchar(100) null,
Estado varchar(14)  null
constraint Estado Check (Estado IN ('Administracion', 'Aceptado', 'Rechazado', 'En Divicion', 'Expirado', 'En Direccion', 'Por capturar')),
QUIENAUTORIZA Nvarchar(100) null,
QuienAutorizaDireccion Nvarchar(100) null, 
foreign key (NoEmpPro) references Usuarios(NoEmpleado)
)

insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040411HL','Armando','Zambrano','Juarez','Docente')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('2025010070','Angel','Sanchez','Jimenez','Docente')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040411HS','Armando','Zambrano','Juarez','Administrativo')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA0104042HS','Angel','Zambrano','Juarez','Administrativo')

insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040211HS','Armando','Zambrano','Juarez','Director')
insert into Usuarios(NoEmpleado, Nombre, ApellidoPaterno, ApellidoMaterno, TipoUsuario) values('ZAJA01040212HS','Armando','Zambrano','Juarez','JefeDP/Div')




ALTER TABLE PERMISO 
ALTER COLUMN AREAADSCRIPCION NVARCHAR(100);


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
                                      "From PERMISO PEM inner join Usuarios Us on Us.NoEmpleado = PEM.NoEmpPro WHERE Us.NoEmpleado = 'SAJA020118HSLNMNA8'


SELECT *FROM PERMISO

EXEC sp_help PERMISO;
