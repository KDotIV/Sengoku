import {createStore, applyMiddleware, compose } from 'redux';
import { persistStore } from 'redux-persist';
import rootReducer from './reducers/rootIndex';
import thunk from 'redux-thunk';

const composeEnhancer = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ || compose;
let store = null;

export const loadState = () => {
    try {
        const serializeState = sessionStorage.getItem('state');
        if(serializeState === null) {
            return undefined;
        } else {
            console.log("State Loaded");
            return JSON.parse(serializeState);
        }
    } catch (err) {
        console.log("State didn't Load");
        return undefined;
    }
}

export const saveState = (state) => {
    try {
        const serializeState = JSON.stringify(state);
        localStorage.setItem('dataLake', serializeState);
        console.log(JSON.stringify(state));
    } catch (err) {
        console.log("state failed to save");
    }
}

if(process.env.NODE_ENV === 'development') {
    store = createStore(
        rootReducer,
        loadState(),
        composeEnhancer(applyMiddleware(thunk))
        );
} else {
    store = createStore(rootReducer, loadState(), applyMiddleware(thunk));
}

export const persistedStore = persistStore(store);

export default store;
