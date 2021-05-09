import React from "react";
import { Route, Switch } from 'react-router-dom';

import { Home } from "./pages/Home";
import { Error } from "./pages/Error";
import Events from "./pages/Events";
import Organizers from './pages/Organizers';
import Login from "./pages/Login";
import SingleEvent from "./pages/SingleEvent";

import NavBar from "./components/Navbar";


function App() {
  return (
    <>
    <NavBar />
    <Switch>
      <Route exact path="/" component={Home}/>
      <Route exact path="/events" component={Events}/>
      <Route exact path="/events/:id" component={SingleEvent}/>
      <Route exact path="/organizers" component={Organizers}/>
      <Route exact path="/login" component = {Login} />
      <Route component={Error}/>
    </Switch>
    </>
  );
}

export default App;
