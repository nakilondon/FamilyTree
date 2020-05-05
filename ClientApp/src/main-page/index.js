import React, { Component } from 'react';
import AppPresentation from './app-presentation';


export default class App extends Component {
  state = {setView: this.setView}

  componentDidMount(){
    this.setState({activePerson: "XREF1"});
    this.setState({activeView: "FamilyTree"})
  }

  selectedPerson = (person) => {
    this.setState({activePerson: person});
  }

  selectedView = (view) => {
    this.setState({activeView: view});
  }

  setView(view ) {
    this.setState({activeView: view});
  }

  render () {
    const view = "FamilyTree";
    return (
          <AppPresentation selectedPerson={this.selectedPerson} 
            activePerson={this.state.activePerson}
            selectedView={this.selectedView}
            activeView={this.state.activeView}/>
    );
  }
}