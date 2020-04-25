import React, { Component } from 'react';
import AppPresentation from './app-presentation'

export default class App extends Component {
  state = {}

  componentDidMount(){
    this.setState({activePerson: "XREF1"});
  }

  selectedPerson = (person) => {
    this.setState({activePerson: person});
  }

  render () {
    return (
        <AppPresentation selectedPerson={this.selectedPerson} 
        activePerson={this.state.activePerson}/>
    );
  }
}