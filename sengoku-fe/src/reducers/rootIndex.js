import { combineReducers } from 'redux';
import eventsReducer from './eventsReducer';
import playersReducer from './playersReducer';

const rootReducer = combineReducers({
    events: eventsReducer,
    players: playersReducer,
});

export default rootReducer;
