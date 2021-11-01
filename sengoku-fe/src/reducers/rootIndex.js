import { combineReducers } from 'redux';
import { persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage';
import eventsReducer from './eventsReducer';
import playersReducer from './playersReducer';
import legendsReducer from './legendsReducer';
import lastAction from './lastAction';

const persistConfig = {
    key: 'root',
    storage,
    whitelist: ['legends']
}

const rootReducer = combineReducers({
    events: eventsReducer,
    players: playersReducer,
    legends: legendsReducer,
    lastAction: lastAction,
});

export default persistReducer(persistConfig, rootReducer);
