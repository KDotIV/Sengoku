import { combineReducers } from 'redux';
import eventsReducer from './eventsReducer';
import playersReducer from './playersReducer';
import legendsReducer from './legendsReducer';

const rootReducer = combineReducers({
    events: eventsReducer,
    players: playersReducer,
    legends: legendsReducer,
});

export default rootReducer;
