<p align="center">
  <img src="Documentation/SmartChaosMachines_Logo.png" alt="Smart Chaos Machines Logo" width="220" />
</p>

# **Smart Chaos Machines: Solution Overview**

**Smart Chaos Machines** is a next generation optimization platform designed to revolutionize factory packing operations. Currently, factory operators manually configure counting machines for each production order based on brick type and production requirements. It is a time-intensive process that relies heavily on operator experience and intuition.

The solution automates this configuration process by collecting real-time performance data from counting machines via OPC-UA protocol, analyzing historical patterns, and applying machine learning algorithms to determine optimal settings. The system will automatically configure counting machines during production changeovers, reducing manual intervention while improving throughput and consistency.

**Key capabilities include:**

- Real-time monitoring and control of counting machines across production lines
- Automated configuration management based on production orders and element characteristics
- Comprehensive observability dashboards for operators, developers, and data scientists
- Machine learning-driven optimization that continuously improves based on performance feedback
- Global deployment architecture supporting factories worldwide with cloud-based universal monitoring

The platform transforms counting machine operations from reactive, manual processes to proactive, data-driven automation—ultimately reducing changeover times, minimizing human error, and establishing a foundation for continuous performance optimization across the global manufacturing network.

## 📋 **Project Documentation**

### Core Project Documents

- **[📄 Project Proposal](Documentation/Project-Proposal.md)** - Complete project overview, architecture decisions, and risk analysis
- **[🚀 Implementation Plan](Documentation/Implementation-Plan.md)** - Detailed sprint breakdown and phased implementation approach
- **[👥 Ways of Working](Documentation/Ways-of-Working.md)** - Team collaboration practices and methodologies

### Quick Navigation

| Need to understand...                         | Read this document                                                                                                |
| --------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| **What we're building and why**               | [Project Proposal](Documentation/Project-Proposal.md)                                                             |
| **How we're building it (timeline & phases)** | [Implementation Plan](Documentation/Implementation-Plan.md)                                                       |
| **How the team collaborates**                 | [Ways of Working](Documentation/Ways-of-Working.md)                                                               |
| **Technical architecture**                    | [Project Proposal - Architecture Decisions](Documentation/Project-Proposal.md#architecture-decisions)             |
| **Sprint planning process**                   | [Ways of Working - Visual Planning](Documentation/Ways-of-Working.md#visual-planning-with-architecture-snapshots) |
| **Risk management**                           | [Project Proposal - Key Risks](Documentation/Project-Proposal.md#key-risks)                                       |
| **Success metrics**                           | [Project Proposal - Success Metrics](Documentation/Project-Proposal.md#success-metrics)                           |

---

## GDPR and Data Handling Notice

This platform is designed to comply fully with GDPR and relevant data protection regulations, especially for deployments across the European Union. Key points to note:

- **Data Minimization:** We collect only the data necessary for the platform's purpose, including machine telemetry and configuration metadata.
- **Personal Data Handling:** If any operator-related personal data (such as login events or audit trails) are captured, processing is strictly governed by explicit consent and data subject rights.
- **Data Sovereignty:** Production and operator data processed on-premises for real-time operations, with aggregated, anonymized data streamed to compliant cloud services for global analytics.
- **Access and Transparency:** Operators can request access to their data and exercise rights to correct or delete data, consistent with GDPR requirements.
- **Security Measures:** Data is encrypted in transit and at rest, with robust access controls and audit logging.
- **Privacy by Design:** Privacy and data protection principles are embedded throughout the system design and development lifecycle.

For detailed information on GDPR compliance and data handling policies, see the [Project Proposal - GDPR and Data Handling section](Documentation/Project-Proposal.md#gdpr-and-data-handling).

---

## Architecture Overview

### Hybrid Edge-Cloud Architecture

The Smart Chaos Machines platform uses a **hybrid edge-cloud architecture** that keeps real-time control on-premises while enabling universal monitoring through a cloud-based data lake.

- **Edge Processing**: Real-time OPC-UA communication and machine control remain on factory premises
- **Cloud Data Lake**: Aggregated data streams to cloud for global analytics and cross-factory insights
- **Universal Monitoring**: Single dashboard view across all global factories

_For complete architectural details, see [Project Proposal - Architecture Decisions](Documentation/Project-Proposal.md#architecture-decisions)._

### Architecture Diagrams

![C4 Level 1 - System Diagram](Documentation/Architecture%20Diagrams/C4%20Level%201%20-%20System%20Diagram.png)
<sub>**Diagram Reference:** This system diagram is also created using the <a href="https://c4model.com/" target="_blank">C4 model</a> for visualizing software architecture.</sub>

![C4 Level 2 - Container Diagram](Documentation/Architecture%20Diagrams/C4%20Level%202%20-%20Container%20Diagram.png)
<sub>**Diagram Reference:** This architecture diagram is created using the <a href="https://c4model.com/" target="_blank">C4 model</a> for visualizing software architecture.</sub>

---

## **How We Work**

The team operates in **2-week agile sprints** with a focus on **continuous pairing** and **architecture-driven planning**. Our approach emphasizes:

- **Pair Programming as Default**: All production code written in pairs to maximize knowledge distribution
- **Visual Planning**: Architecture diagram snapshots drive sprint planning and task breakdown
- **Phase-Based Development**: Clear phase boundaries from foundation through global rollout

### Quick Team References

- **Current Phase**: _[Check Implementation Plan](Documentation/Implementation-Plan.md#work-methodology-agile-with-phased-focus)_
- **Sprint Planning Process**: _[Ways of Working - Visual Planning](Documentation/Ways-of-Working.md#visual-planning-with-architecture-snapshots)_
- **Pairing Guidelines**: _[Ways of Working - Pair Programming](Documentation/Ways-of-Working.md#pair-programming-as-default-practice)_
- **Team Health Practices**: _[Ways of Working - Team Health](Documentation/Ways-of-Working.md#team-health-and-morale)_

For complete team collaboration details, see [Ways of Working](Documentation/Ways-of-Working.md).

---

## **Project Structure**

> **Tech Stack:** This project is built with **.NET 10** and follows the principles of **CLEAN architecture** for maintainability, scalability, and testability.

### Implementation Status

**Current Implementation Phase**: _[View in Implementation Plan](Documentation/Implementation-Plan.md)_

**Success Metrics Tracking**: _[View targets in Project Proposal](Documentation/Project-Proposal.md#success-metrics)_

---

## Troubleshooting

### Browser Certificate Error

<https://learn.microsoft.com/en-us/dotnet/aspire/troubleshooting/untrusted-localhost-certificate>

1. Close all browser windows
2. Run `dotnet dev-certs https --clean`
3. Run `dotnet dev-certs https --trust`
4. Rebuild solution and run

---

## Getting Started

1. **Understand the Project**: Start with [Project Proposal](Documentation/Project-Proposal.md)
2. **Check Current Sprint**: Review [Implementation Plan](Documentation/Implementation-Plan.md)
3. **Join the Team**: Read [Ways of Working](Documentation/Ways-of-Working.md) for collaboration practices
4. **Set up Development Environment**: Follow technical setup instructions above

_For questions about the project, architecture, or team practices, refer to the relevant documentation sections linked above._
