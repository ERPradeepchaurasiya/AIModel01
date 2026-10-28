using System.Collections.Generic;

namespace ModelTrainer
{
    public static class Dataset
    {
        public static List<JJMInfo> GetDigitalGrowthDataset()
        {
            var data = new List<JJMInfo>();

            void Add(string[] queries, string response)
            {
                foreach (var q in queries)
                {
                    data.Add(new JJMInfo
                    {
                        User = q,
                        Assistant = response
                    });
                }
            }

            // 1. Greetings & Introductions
            Add(
                new[] { "hi", "hello", "hey", "hello there", "good morning", "good evening", "greetings" },
                "Hello! Welcome to Digital Growth IT Software Solution. | We specialize in custom software development, web & mobile applications, MLM software, and ERP systems. | How can we assist you today? | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "who are you", "what is your name", "what can you do", "introduce yourself", "tell me what you can do" },
                "I am the Digital Growth IT Assistant. | I can help you with information about our software development services, website design, mobile apps, MLM solutions, ERP/CRM, pricing, and support. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 2. Company Overview
            Add(
                new[] {
                    "who is digital growth it software solution", "about digital growth it", "tell me about your company",
                    "what is digital growth it", "company background", "what does your company do", "about digitalgrowth"
                },
                "Digital Growth IT Software Solutions Private Limited is a premier IT and software development company based in Lucknow, Uttar Pradesh. | We provide end-to-end technology solutions including custom software, web design, mobile apps, MLM software, school/college ERPs, GST billing, and digital marketing. | Steps: 1. Explore our portfolio 2. Consult with our technical team 3. Get a customized project quote. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 3. Contact & Location Details
            Add(
                new[] {
                    "what is your contact number", "phone number", "how to call you", "call support",
                    "contact info", "how can i contact digital growth", "customer care number", "mobile number"
                },
                "You can reach Digital Growth IT Software Solution at: | Phone: +91-8188884173 | Alternate: +91-8188884176 | Email: contact@digitalgrowthitsoftwaresolution.com or digitalgrowthit@gmail.com | Steps: Call during business hours (10:00 AM - 7:00 PM) or send a WhatsApp message. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] {
                    "where is your office located", "company address", "office location", "office address",
                    "where are you located", "lucknow office address", "visit your office"
                },
                "Our registered office is located at: | L-4/87, Sector - M, Aliganj, Lucknow, Uttar Pradesh, India - 226024. | You are welcome to visit our office for in-person project discussions. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "what is your email address", "email id", "send email", "contact email" },
                "You can email our team directly at: | Primary: contact@digitalgrowthitsoftwaresolution.com | Secondary: digitalgrowthit@gmail.com | We typically respond within 24 business hours. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 4. Custom Website Development
            Add(
                new[] {
                    "website development", "web design services", "build a website", "create website",
                    "custom web development", "corporate website", "dynamic website", "responsive website development"
                },
                "Digital Growth IT builds high-performance, modern, and responsive websites tailored to your brand. | Services include: Corporate websites, dynamic portals, responsive web design, CMS development, and web application development. | Technologies: .NET Core, React, Angular, Node.js, PHP, Bootstrap, and SQL Server. | Steps: 1. UI/UX mockup design 2. Frontend & backend development 3. Quality assurance testing 4. Domain & hosting launch. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 5. Mobile App Development
            Add(
                new[] {
                    "mobile app development", "android app development", "ios app", "create mobile application",
                    "flutter app", "react native app", "smartphone application development", "app developer"
                },
                "We develop robust native and cross-platform mobile apps for Android and iOS. | Capabilities: Intuitive UI/UX, real-time push notifications, payment gateway integration, secure RESTful APIs, and Google Play / Apple App Store publishing. | Technologies: Android Java/Kotlin, Flutter, React Native, and Swift. | Steps: 1. Wireframing & architecture 2. Native/cross-platform coding 3. API integration 4. Store deployment. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 6. E-Commerce Solutions
            Add(
                new[] {
                    "ecommerce development", "online shopping website", "ecommerce portal", "create online store",
                    "multi vendor marketplace", "shopping cart development", "payment gateway integration"
                },
                "We deliver scalable and conversion-focused E-Commerce platforms. | Features: Product catalog, shopping cart, multi-vendor support, customer wishlists, automated order tracking, and coupons. | Payment Gateways: Razorpay, Paytm, Stripe, PhonePe, and Cash on Delivery (COD). | Steps: 1. Store catalog design 2. Cart & checkout workflow 3. Payment & SMS gateway setup 4. Go-live. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 7. MLM Software Development (Key Specialization)
            Add(
                new[] {
                    "mlm software development", "mlm software", "multi level marketing software",
                    "network marketing software", "mlm web application", "best mlm software developer"
                },
                "Digital Growth IT is an industry leader in customized MLM (Multi-Level Marketing) Software. | Features: Interactive genealogy tree, dynamic member dashboard, automated payout generation, e-pin generation, member-to-member fund transfer, and SMS alerts. | Supported Plans: Binary, Matrix, Single Leg, Generation, Board, and Helping/Crowdfunding. | Steps: 1. Plan structure consultation 2. Commission calculation rules 3. Dashboard demo 4. Secure deployment. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "binary mlm software", "binary plan software", "binary tree mlm" },
                "Our Binary MLM software supports 1:1 and 1:2 pairing, power leg carry-forward, capping limits, and automated pairing income calculations. | Includes real-time visual genealogy tree and automated TDS/Admin deduction handling. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "matrix mlm software", "matrix plan", "forced matrix software" },
                "We build Forced Matrix MLM software with customizable dimensions (e.g. 2x2, 3x3, 4x7 matrix). | Features automatic spillover system, matrix completion bonuses, and fast-track reward tracking. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "single leg mlm software", "single leg plan", "monoline mlm" },
                "Our Single Leg (Monoline) MLM software arranges all members in a single global chain. | Features downline count calculation, direct referral unlock conditions, and daily ROI/level income payouts. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "helping mlm software", "crowdfunding mlm", "donation mlm plan", "gift plan software" },
                "We provide secure Helping and Crowdfunding MLM software with automated give-and-take donation matching. | Features commitment slips, timer-based payment confirmations, and scam-proof security. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 8. ERP Solutions (Schools, Colleges & Institutes)
            Add(
                new[] {
                    "school erp software", "college management system", "institute erp",
                    "student management system", "school management software", "education erp"
                },
                "Our Educational ERP is an all-in-one software for schools, colleges, and coaching institutes. | Modules: Student admissions, biometric/RFID attendance, fee collection with receipt printing, examination & report card generation, library tracking, and parent mobile app. | Steps: 1. Campus requirement analysis 2. Student & fee data migration 3. Staff training 4. Live onboarding. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 9. CRM Solutions
            Add(
                new[] {
                    "crm software", "customer relationship management", "lead management software",
                    "sales crm", "custom crm development"
                },
                "Streamline your sales and client relationships with Digital Growth IT's custom CRM. | Features: Lead capture from Facebook/Google/Website, automated lead assignment, call logging, pipeline stages, quotation generation, and executive performance analytics. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 10. Billing, Accounting & GST Software
            Add(
                new[] {
                    "billing software", "accounting software", "gst invoice software",
                    "retail billing software", "medical billing software", "pos software", "pharmacy software"
                },
                "We offer GST-compliant billing, POS, and inventory accounting software. | Editions available for: Retail stores, wholesale distributors, supermarkets, and medical/pharmacy shops (with batch & expiry management). | Features: Barcode scanning, thermal receipt printing, GST tax reports (GSTR-1, GSTR-3B), and ledger management. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 11. Microfinance & Nidhi Software
            Add(
                new[] {
                    "nidhi company software", "microfinance software", "society software",
                    "loan management software", "rd fd software", "pigmy collection software"
                },
                "Specialized software designed for Nidhi Companies, Cooperative Credit Societies, and Microfinance institutions. | Features: Member share allocation, Savings/RD/FD accounts, personal/gold/property loan disbursement, automated EMI schedules, daily collection mobile app, and RBI compliance reporting. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 12. Real Estate Management Software
            Add(
                new[] { "real estate software", "property management portal", "builder software", "property listing website" },
                "Custom software for real estate developers, builders, and property brokers. | Features: Project plot/unit layout mapping, customer booking status, installment/EMI tracking, broker commission management, and property search portal. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 13. News Portal & Media Websites
            Add(
                new[] { "news portal development", "news website", "online newspaper portal", "media cms" },
                "High-speed, SEO-friendly digital news portals built for media publications and journalists. | Features: Breaking news alerts, video embedding, YouTube live streaming, multi-category layout, social sharing, and Google AdSense optimization. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 14. Restaurant Management & POS
            Add(
                new[] { "restaurant software", "restaurant billing pos", "kot software", "cafe billing" },
                "End-to-end POS and management solution for restaurants, cafes, and cloud kitchens. | Features: Table reservations, kitchen order ticketing (KOT), recipe inventory tracking, split billing, and Zomato/Swiggy order integration. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 15. Digital Marketing & SEO
            Add(
                new[] {
                    "digital marketing services", "seo services", "search engine optimization",
                    "social media marketing", "smo services", "google ads ppc", "grow business online"
                },
                "Accelerate your online reach with our full-suite Digital Marketing services. | Services: Search Engine Optimization (On-page & Off-page SEO), Google Ads (PPC), Facebook/Instagram ad campaigns, Social Media Optimization (SMO), and content marketing. | Steps: 1. Competitor & keyword audit 2. Campaign strategy 3. Ad optimization 4. Monthly ROI performance report. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 16. Bulk SMS & Email Marketing
            Add(
                new[] { "bulk sms service", "transactional sms", "promotional sms", "whatsapp marketing", "bulk email service" },
                "Reach thousands of customers instantly with our Bulk SMS and Email marketing platform. | Services: Promotional SMS, DLT-approved Transactional SMS, OTP delivery APIs, WhatsApp Business campaigns, and targeted bulk email marketing. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 17. Web Hosting & Domain Services
            Add(
                new[] { "web hosting services", "vps hosting", "cloud server", "domain registration", "buy domain and hosting" },
                "Reliable, high-speed, and secure hosting infrastructure for your websites and applications. | Offerings: Linux cPanel hosting, Windows IIS hosting, Cloud VPS, SSL certificates, business email accounts, and 99.9% uptime guarantee. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 18. Pricing, Quotations & Process
            Add(
                new[] {
                    "how much does a website cost", "project price", "pricing", "cost of mlm software",
                    "cost of software development", "get a quotation", "how to get quote", "request proposal"
                },
                "Our software pricing is transparent, competitive, and customized based on your specific requirements and scope. | We provide fixed-price project quotes as well as monthly milestones. | Steps: 1. Share your project requirements 2. Receive a free detailed proposal and timeline estimate 3. Approve and start development. | Contact sales: +91-8188884173 | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "what is your development process", "how do you work", "development lifecycle", "project steps" },
                "Our development methodology follows a 5-step Agile lifecycle: | Steps: 1. Requirement Analysis & Specification 2. UI/UX Wireframing & Prototype Approval 3. Agile Core Development 4. Rigorous QA, Security & Performance Testing 5. Deployment, Staff Training & Ongoing Support. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 19. Support & Maintenance
            Add(
                new[] { "do you provide maintenance", "technical support", "software amc", "after sales support", "customer support" },
                "Yes! We provide comprehensive post-delivery support and Annual Maintenance Contracts (AMC). | Services: Bug fixing, server maintenance, database backups, security patches, feature upgrades, and dedicated phone/email support. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 20. Technology Stack
            Add(
                new[] { "what technologies do you use", "tech stack", "programming languages", "frameworks used" },
                "We work with leading modern enterprise technologies: | Backend: C# .NET Core, ASP.NET MVC, PHP, Node.js, Python | Frontend: React, Angular, Vue.js, HTML5, CSS3, Bootstrap | Mobile: Flutter, Android (Kotlin/Java), React Native, iOS (Swift) | Databases: Microsoft SQL Server, MySQL, PostgreSQL, MongoDB | Cloud: AWS, DigitalOcean, Microsoft Azure. | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            // 21. Thank you & Goodbye
            Add(
                new[] { "thank you", "thanks", "thank you very much", "appreciate your help" },
                "You are very welcome! If you have any further questions or would like to discuss a project, feel free to ask or call us at +91-8188884173. Have a wonderful day! | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            Add(
                new[] { "bye", "goodbye", "see you later", "exit" },
                "Goodbye! Thank you for visiting Digital Growth IT Software Solution. We look forward to working with you! | Official link: https://digitalgrowthitsoftwaresolution.com/"
            );

            return data;
        }
    }
}
