import React, { Component } from 'react';

export default class PersonTable extends Component {
    state ={}
    constructor(props) {
        super(props);
    }

    onPersonChange(member){
        const id = member.id;
        this.props.selectedPerson(id);
      }
    
    relationShip(type){
        if (type == 0){
          return "Spouse";
        }
    
        if (type == 1){
          return "Parent";
        };
    
        if (type == 2){
          return "Child";
        };
    
        if (type == 3){
          return "Sbling";
        };
    }
      
    
    buildFamilyTable(Family) {
        if (Family == null)
        {
           return <tr><td>No Family</td></tr>
        }
    
        return (
          Family
            .sort((a,b) => a.type - b.type)
            .map(member =>
                <tr key={member.id} value={member.id} onClick={() => this.onPersonChange(member.id)}>
                  <td>{this.relationShip(member.type)}</td>
                  <td >{member.label}</td>
                </tr>)
        );
    }
    
    buildEvents(Events) {
        if (Events == null)
        {
          return <tr key="blank"><td>No events</td></tr>
        }
          
        return (
          Events.map(event =>
            <tr key={event.eventDate}>
              <td>{event.detail}</td>
              <td>{event.eventDate}</td>
              <td>{event.place}</td>        
            </tr>)
        );
    }


    render() {
        return (
            <div>
                <h1 id="tabelLabel" >{this.props.details.title}</h1>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <tbody>
                        {this.buildFamilyTable(this.props.details.family)}
                        {this.buildEvents(this.props.details.events)}
                    </tbody>
                </table>
            </div>
        );
    }
}