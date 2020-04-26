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
    
    buildFamilyTable(Family) {
        if (Family == null)
        {
           return <tr><td>No Family</td></tr>
        }
    
        return (
          Family
            .map(member =>
                <tr key={member.id} value={member.id} onClick={() => this.onPersonChange(member.id)}>
                  <td>{member.relationship}</td>
                  <td >{member.name}</td>
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