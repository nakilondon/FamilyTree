import React, { Component } from 'react';

export default class PersonDetails extends Component {
  static displayName = PersonDetails.name;
  
  constructor(props) {
    super(props);
    this.state = { Details: [], loading: true, id: this.props.id };
  }

  componentDidMount() {
    this.populatePersonData(this.props.id);
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
  

  buildFamilyTable(Family, onClick, relationShip) {
    if (Family == null)
    {
       return <tr><td>No Family</td></tr>
    }

    return (
      Family
        .sort((a,b) => a.type - b.type)
        .map(member =>
            <tr key={member.id} value={member.id} onClick={() => onClick(member)}>
              <td>{relationShip(member.type)}</td>
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
        </tr>));
  }

  static renderDetailTable(Details, onClick, buildFamilyTable, relationShip, buildEvents) {
        
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">

        <tbody>
          {buildFamilyTable(Details.family, onClick, relationShip)}
          {buildEvents(Details.events)}
        </tbody>
      </table>
      
    );
  }

  render() {
    if (this.state.id != this.props.id)
    {
      this.setState({loadding: true, id: this.props.id});
      this.populatePersonData(this.props.id);      
    }

    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : PersonDetails.renderDetailTable(this.state.Details, this.onPersonChange, this.buildFamilyTable, this.relationShip, this.buildEvents);

    return (
      <div className="col-md-2">
        <h1 id="tabelLabel" >{this.state.Details.title}</h1>
        {contents}
      </div>
    );
  }

  async populatePersonData(id) {
    const response = await fetch(`familytree/${id}`);
    const data = await response.json();
    this.setState({ Details: data, loading: false });
  }
}