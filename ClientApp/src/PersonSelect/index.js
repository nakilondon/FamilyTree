import React, {Component} from 'react'

export default class PersonSelect extends Component {
    state = {}

    constructor(props) {
        super(props);
        this.state = { People: [], loading: true};
    }

    componentDidMount() {
        this.populateSelectData();
    }

    onSeachChange= (e) => {
        const id = e.target.value;
        this.props.selectedPerson(id);
    }
    
    static renderSelect(people, onSeachChange, currentValue) {
        return (
            <div className="form-group row mt-3">
                <div className="offset-md-1 col-md-2">
                    Select
                </div>
                <div className="col-md-9">
                    <select className="form-control" value={currentValue} onChange={onSeachChange}>
                        {people.map((p) => <option key={p.id} value={p.id}>{p.label}</option>)}
                    </select>
                </div>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : PersonSelect.renderSelect(this.state.People, this.onSeachChange, this.props.id);
  
      return (
        <div className="form-group row mt-3">
            {contents}
        </div>
      );
    }

    async populateSelectData() {
        const response = await fetch(`familytree/list`);
        const data = await response.json();
        this.setState({ People: data, loading: false });
    }
}