import { Observable } from 'rxjs';
import { Movimiento } from '../../models/Movimiento';
import { MovimientoDTO } from '../../dtos/movimiento.dto';

export interface IMovimientoService {
  getMovimientos(): Observable<MovimientoDTO[]>;
  getMovimientoById(id: number): Observable<Movimiento>;
  createMovimiento(movimientoDto: MovimientoDTO): Observable<Movimiento>;
  updateMovimiento(id: number, movimientoDto: MovimientoDTO): Observable<Movimiento>;
  deleteMovimiento(id: number): Observable<void>;
}
