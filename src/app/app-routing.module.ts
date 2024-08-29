import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { AdministradorComponent } from './administrador/administrador.component';
import { GestorComponent } from './gestor/gestor.component';
import { PrincipalComponent } from './principal/principal.component';
import { GestorproyectosComponent } from './gestorproyectos/gestorproyectos.component';
import { DatosgestorproyectosComponent } from './datosgestorproyectos/datosgestorproyectos.component';
import { UsuariosprincipalComponent } from './usuariosprincipal/usuariosprincipal.component';
import { UsuariosasiganarliderComponent } from './usuariosasiganarlider/usuariosasiganarlider.component';
import { UsuariosasignarComponent } from './usuariosasignar/usuariosasignar.component';

const routes: Routes = [
 { path: 'login', component: LoginComponent},
 { path: 'administrador', component: AdministradorComponent},
 { path: 'gestor', component: GestorComponent},
 { path: 'principal', component: PrincipalComponent},
 { path: 'gestorproyectos', component: GestorproyectosComponent},
 { path: 'datosproyecto', component: DatosgestorproyectosComponent},
 { path: 'usuariolider', component: UsuariosprincipalComponent},
 { path: 'liderprincipal', component: UsuariosasiganarliderComponent},
 { path: 'datosliderproyecto', component: UsuariosasignarComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
