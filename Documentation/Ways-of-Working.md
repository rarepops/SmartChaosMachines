# Ways of Working: Smart Chaos Machines Team

_This document defines team collaboration practices for the Smart Chaos Machines project. For project context and technical architecture, see [Project Proposal](Project-Proposal.md). For implementation timeline, see [Implementation Plan](Implementation-Plan.md)._

## **"Energize People Everyday":**

- **Dad Joke Standups**: Dad jokes before the standup starts to start meetings with a bit of fun
- **Gaming Sessions**: Regular online gaming with mic-required games (like Lethal Company) to build natural communication and teamwork (outside the pressure of production deadlines, with approval by manager)
- **Tech Fact Standups**: Start standups with interesting industrial automation or tech facts
- **Sprint Celebrations**: Special activities when major milestones are achieved (first successful CM connection, automated configuration working, etc.)

## Philosophy

Our approach to team collaboration is built on two core principles: **continuous pairing** and **architecture-driven planning**. These practices address the unique challenges of our distributed team working on complex industrial IoT systems while building the psychological safety and technical excellence needed for success.

_These practices directly support the implementation phases outlined in [Implementation Plan](Implementation-Plan.md) and the technical complexity described in [Project Proposal](Project-Proposal.md)._

## Pair Programming as Default Practice

### Why Pair Programming

**For Our Context:**

- **Knowledge Distribution**: Critical when dealing with OPC-UA integration, firmware complexities, and external specialists - prevents single points of failure
- **Team Cohesion**: Merged teams with different technical backgrounds benefit from forced collaboration and shared problem-solving
- **Continuous Learning**: Natural knowledge transfer between team members with different expertise areas

**Alignment with Project Needs:** _Supporting the modular monolithic architecture and multi-phase implementation described in [Project Proposal](Project-Proposal.md#modular-monolithic-architecture)_

### Implementation

**Daily Operations:**

- **Default to Pairing**: All production code written in pairs, especially OPC-UA integration and ML pipeline work, where there might be knowledge gaps
- **Daily Pair Rotation**: Switch pairs daily to maximize knowledge distribution across the team
- **Driver/Navigator Model**: Clear responsibilities to prevent clashes, especially important with mixed experience levels
- **Solo Work Exceptions**: Research spikes, documentation, and individual learning tasks only

**Special Sessions:**

- **Mob Programming**: Entire team tackles complex challenges together (firmware integration)
- **Cross-Functional Pairing**: Developers pair with external specialists for ML integration work
- **Critical Code Pairing**: Mandatory pairing for any code touching counting machines

**Remote Pairing:**

- **VS Code Live Share**: Seamless pairing for any distributed team members
- **mob.sh + Screen Share**: Lightweight tool for seamless pair programming handoffs

## Visual Planning with Architecture Snapshots

### Architecture-Driven Sprint Planning

_Supporting the phased implementation approach detailed in [Implementation Plan](Implementation-Plan.md#work-methodology-agile-with-phased-focus)_

**Process:**

1. **Current State Review**: Start each sprint with updated architecture diagram snapshot showing what's built, being built, or needs building - keeping planning visual and immediately actionable
2. **Target State Vision**: Visualize what the architecture should look like after the sprint
3. **Gap Analysis**: Identify components that need building/modification
4. **Dependency Mapping**: Draw connections between tasks affecting the same components
5. **Risk Visualization**: Highlight integration points that could cause blockers

**Planning Sessions:**

- **Sprint Planning**: Architecture diagram drives task breakdown and estimation
- **Daily Standups**: Brief architecture context for blockers and dependencies
- **Retrospectives**: Architecture evolution discussion - what changed and why

### Visual Tools and Techniques

**Digital Collaboration:**

- **Miro**: Collaborative architecture sessions and planning board
- **Component Ownership**: Color-code architecture components by team member expertise
- **Integration Highlights**: Clearly mark OPC-UA, RabbitMQ, and external API endpoints
- **Deployment Boundaries**: Visual separation of edge, on-premises, and cloud components

_Directly supporting the hybrid edge-cloud architecture described in [Project Proposal](Project-Proposal.md#hosting-decisions-hybrid-edge-cloud-architecture)_

**Living Documentation:**

- **Architecture as Code**: Diagrams version-controlled and updated each sprint
- **Component Maps**: Visual representation of which team members have expertise where, and possibly interests in learning
- **Dependency Tracking**: Updated visualization as external API integrations are discovered

## Knowledge Sharing and Documentation

### Continuous Knowledge Sharing

**Documentation Practices:**

- **Pair Programming Sessions**: Document architectural decisions made during pairing
- **Architecture Evolution**: Track changes and rationale in living documents
- **External Specialist Knowledge**: Mandatory capture of knowledge before contractor rotations
- **Cross-Team Learning**: Regular tech talks on domain expertise

**Knowledge Distribution:**

- **No Single Points of Failure**: Ensure at least two people understand each critical system component
- **Onboarding Through Pairing**: New team members paired with different experts daily
- **External Integration Expertise**: Shared understanding of external APIs

_Critical for managing the risks identified in [Project Proposal](Project-Proposal.md#organizational-risks)_

## Team Health and Morale

### Building Psychological Safety

**Collaboration:**

- **Shared Ownership**: All code belongs to the team, not individuals
- **No Blame Culture**: System failures focus on improvement, not finding faults
- **Learning Opportunities**: Mistakes become team learning sessions

**Recognition and Progress:**

- **Small Wins Celebration**: First successful CM connection, first automated configuration
- **Visible Progress**: Shared dashboards showing sprint completion and system uptime
- **Team Achievements**: Document and share successes with broader organization

### Balanced Workload

**Support Structure:**

- **Firefighter Rotation**: One developer per sprint handles legacy applications
- **Pair Support**: Firefighter never works alone - always has backup
- **Technical Debt Time**: 10% sprint capacity for unforeseen issues
- **Learning Budget**: Time allocated for exploring relevant new technologies

_Mitigating legacy application neglect risk identified in [Project Proposal](Project-Proposal.md#organizational-risks)_

## Success Indicators

### Team Health Metrics

- **Regular Satisfaction Surveys**: Tracking upward trend in team morale
- **Reduced Decision Time**: Faster resolution of technical decisions through collaboration
- **Increased Participation**: Active engagement in retrospectives and planning sessions
- **Proactive Communication**: Team members surface blockers early and often

### Collaboration Quality

- **Knowledge Distribution**: Multiple people can handle any system component
- **Voluntary Challenging Work**: Team members choose difficult tasks rather than avoiding them
- **Shared Problem Ownership**: Production issues addressed collectively, not individually
- **Cross-Team Relationships**: Positive collaboration with Recipe API and Line Configuration teams

### Technical Excellence

- **Code Quality**: Reduced bugs in production due to pair programming
- **Architecture Clarity**: Team can easily explain system design and dependencies
- **Integration Success**: Smooth deployments and fewer rollbacks
- **Continuous Improvement**: Regular architecture refactoring and technical debt reduction

_Supporting the technical excellence metrics defined in [Project Proposal](Project-Proposal.md#technical-excellence)_

## Implementation Timeline

### Week 1-2: Foundation

- Establish pairing rotations and ground rules
- Create initial architecture visualization tools
- Define documentation standards

### Week 3-4: Rhythm Development

- Implement architecture-driven sprint planning
- Establish mob programming sessions
- Refine pairing practices based on early feedback

### Week 5+: Continuous Improvement

- Regular retrospectives on ways of working effectiveness
- Adjust practices based on project needs and team feedback
- Maintain focus on team health alongside technical delivery

_Timeline aligns with Phase 1 foundation work described in [Implementation Plan](Implementation-Plan.md#phase-1-foundation-2-sprints)_

## Integration with Project Phases

**Phase 1-2 (Foundation & Core Value):**

- Heavy pairing on OPC-UA integration
- Architecture snapshots crucial for understanding modular monolith evolution

**Phase 3 (Stabilization):**

- Mob programming for UAT preparation
- Cross-functional pairing with operators for training

**Phase 4 (Global Rollout):**

- Knowledge distribution critical for supporting multiple factories
- Visual planning essential for tracking deployment progress

**Phase 5 (ML Integration):**

- External specialist pairing becomes critical
- Architecture evolution tracking for ML pipeline integration

---

_Related Documents:_

- _[Project Proposal](Project-Proposal.md) - Technical context and architecture these practices support_
- _[Implementation Plan](Implementation-Plan.md) - Sprint timeline and phases these practices enable_
