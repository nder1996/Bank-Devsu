import { Observable } from 'rxjs';
import { Movimiento } from '../../models/Movimiento';
import { MovimientoDto } from '../../dtos/movimiento.dto';


export interface IMovimientoService {
  getMovimientos(): Observable<MovimientoDto[]>;
  getMovimientoById(id: number): Observable<Movimiento>;
  createMovimiento(movimientoDto: MovimientoDto): Observable<Movimiento>;
  updateMovimiento(id: number, movimientoDto: MovimientoDto): Observable<Movimiento>;
  deleteMovimiento(id: number): Observable<void>;
}
